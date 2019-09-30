using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;


namespace ConsoleDriver {
    public class Experiment {
        public interface IExecutor {
            string Name { get; } // Algorithm name
            int Size { get; } // Problem size
            string Result { get; } // Info write to log
            string Solution { get; } // Info write to solution
            void Execute(); // Call solve function
        }
        public enum State {
            Init,Error,Running,Finished
        }

        private State _Now;
        public State Now => _Now;
        
        private IExecutor _Executor;

        public IExecutor Executor => _Executor;

        private DateTime _StartTime;

        public DateTime StartTime => _StartTime;

        private System.Diagnostics.Stopwatch _TimeWatch;
        
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

        public Experiment(IExecutor executor, StreamWriter logStreamWriter = null, 
            StreamWriter solutionStreamWriter = null) {
            _Executor = executor;
            LogStreamWriter = logStreamWriter ?? new StreamWriter(Console.OpenStandardOutput());
            SolutionStreamWriter = solutionStreamWriter ?? StreamWriter.Null;
            LogStreamWriter.AutoFlush = true;
            SolutionStreamWriter.AutoFlush = true;
            _Now = State.Init;
        }

        public void Start() {
            Thread threadToKill = null;
            LogStreamWriter.Write(ExperimentInfo);
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
            LogStreamWriter.Write(ExperimentTimeInfo);
            SolutionStreamWriter.Write(SolutionInfo);
            LogStreamWriter.Write(ResultInfo);
        }

        private void StartWithoutTimeout() {
            _TimeWatch = new Stopwatch();
            _TimeWatch.Start();
            _Now = State.Running;
            _Executor.Execute();
            _TimeWatch.Stop();
            _Now = State.Finished;
        }

        public string ExperimentInfo => $"[INFO] Experiment started in {_StartTime}...\n" +
                                          $"Algorithm name: {_Executor.Name}\n" + 
                                          $"Point set size: {_Executor.Size}\n" + 
                                          $"    Timeout   : {_Timeout / 1000}s.\n";

        public string ExperimentTimeInfo => _Now switch {
            State.Init => $"[INFO] Experiment had not started.\n",
            State.Running => $"[INFO] Experiment is still running.\n",
            State.Error => $"[INFO] Timeout with bound {_Timeout / 1000}s.\n",
            State.Finished => $"[INFO] Experiment finished in {_TimeWatch.Elapsed}.",
            _ => throw new NotSupportedException($"State {_Now} not supported.")
        };

        public string ResultInfo => _Now switch {
            State.Init => $"[INFO] Experiment had not started.\n",
            State.Running => $"[INFO] Experiment is still running.\n",
            State.Error => $"[INFO] No result for timeout.\n",
            State.Finished => _Executor.Result,
            _ => throw new NotSupportedException($"State {_Now} not supported.")
        };
        
        private string SolutionInfo => _Executor.Solution;
        
        private static string TimeoutLogicalError => $"[ERROR] The time should be in [1,86400]s.\n";

    }
}