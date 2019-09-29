using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;


namespace ConsoleDriver {
    public class Experiment {
        public enum State {
            Init,Error,Running,Finished
        }

        private DateTime _StartTime;

        public DateTime StartTime => _StartTime;

        private Core.ClosestPairOfPoints _Algorithm;
        public Core.ClosestPairOfPoints Algorithm => _Algorithm;
        
        private List<Core.Point> _PointSet;
        public IEnumerable<Core.Point> PointSet => _PointSet;

        private State _Now;
        public State Now => _Now;

        private double _Distance;
        public double Distance => _Distance;
        private System.Diagnostics.Stopwatch _TimeWatch;
        public List<Tuple<Core.Point, Core.Point>> BestPairs => _Algorithm.BestPairs;

        private int _Timeout = 60000;
        public int Timeout => _Timeout;

        public StreamWriter LogStreamWriter { get; }
        public StreamWriter SolutionStreamWriter { get; }

        public bool SetTimeout(int time) {
            if (time <= 0 || time > 86400) {
                LogStreamWriter.Write(TimeoutLogicalError);
                return false;
            }
            _Timeout = time * 1000;
            return true;
        }

        public Experiment(Core.ClosestPairOfPoints algorithm, IEnumerable<Core.Point> pointSet, 
            StreamWriter logStreamWriter = null, StreamWriter solutionStreamWriter = null) {
            _Algorithm = algorithm;
            _PointSet = pointSet.ToList();
            LogStreamWriter = logStreamWriter ?? new StreamWriter(Console.OpenStandardOutput());
            SolutionStreamWriter = solutionStreamWriter ?? StreamWriter.Null;
            LogStreamWriter.AutoFlush = true;
            SolutionStreamWriter.AutoFlush = true;
            _Now = State.Init;
        }

        public void Start() {
            Thread threadToKill = null;
            LogStreamWriter.Write(ExperimentInfo());
            _StartTime = DateTime.Now;
            Action wrappedAction = () =>
            {
                threadToKill = Thread.CurrentThread;
                StartWithoutTimeout();
            };
            IAsyncResult result = wrappedAction.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(_Timeout)) {
                wrappedAction.EndInvoke(result);
            }
            else
            {
                threadToKill.Abort();
                _Now = State.Error;
            }
            LogStreamWriter.Write(ExperimentTimeInfo());
            SolutionStreamWriter.Write(SolutionInfo());
            LogStreamWriter.Write(ResultInfo());
        }

        private void StartWithoutTimeout() {
            _TimeWatch = new Stopwatch();
            _TimeWatch.Start();
            _Now = State.Running;
            _Distance = _Algorithm.Solve(_PointSet);
            _TimeWatch.Stop();
            _Now = State.Finished;
        }

        public string ExperimentInfo() => $"[INFO] Experiment started in {_StartTime}...\n" +
                                          $"Algorithm name: {_Algorithm}\n" + 
                                          $"Point set size: {_PointSet.Count}\n" + 
                                          $"    Timeout   : {_Timeout / 1000}s.\n";

        public string ExperimentTimeInfo() => _Now == State.Init ? $"[INFO] Experiment had not started.\n":
            _Now == State.Running ? $"[INFO] Experiment is still running.\n":
            _Now == State.Error ? $"[INFO] Timeout with bound {_Timeout / 1000}s.\n" : 
            $"[INFO] Experiment finished in {_TimeWatch.Elapsed}.";

        public string ResultInfo() => _Now == State.Init ? $"[INFO] Experiment had not started.\n" :
            _Now == State.Running ? $"[INFO] Experiment is still running.\n" :
            _Now == State.Error ? $"[INFO] No result for timeout.\n" :
            Double.IsPositiveInfinity(_Distance)? $"[RESULT] One point set.\n":
            $"[RESULT] Closest distance: {_Distance}.\n" + 
            $"[RESULT] Solution count: {BestPairs.Count}.\n" + 
            $"[RESULT] One solution: {BestPairs[0].Item1} <=> {BestPairs[0].Item2}.\n";
        
        private string SolutionInfo() => String.Join('\n', BestPairs.Select(x => $"{x.Item1} <=> {x.Item2}")) + "\n";
        
        private static readonly string TimeoutLogicalError = $"[ERROR] The time should be in [1,86400]s.\n";

    }
}