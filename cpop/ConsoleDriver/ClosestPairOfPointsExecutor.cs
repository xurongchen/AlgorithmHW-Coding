using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleDriver {
    public class ClosestPairOfPointsExecutor: Experiment.IExecutor {
        private Core.ClosestPairOfPoints _Algorithm;
        public Core.ClosestPairOfPoints Algorithm => _Algorithm;
        
        private List<Core.Point> _PointSet;
        public IEnumerable<Core.Point> PointSet => _PointSet;
        
        private double _Distance;
        public double Distance => _Distance;
        
        public List<Tuple<Core.Point, Core.Point>> BestPairs => _Algorithm.BestPairs;

        public ClosestPairOfPointsExecutor(Core.ClosestPairOfPoints algorithm, IEnumerable<Core.Point> pointSet) {
            _Algorithm = algorithm;
            _PointSet = pointSet.ToList();
        }

        public string Name => _Algorithm.AlgorithmName;

        public int Size => _PointSet.Count;

        public string Result => Double.IsPositiveInfinity(_Distance)
            ? $"[RESULT] One point set.\n"
            : $"[RESULT] Closest distance: {_Distance}.\n" +
              $"[RESULT] Solution count: {BestPairs.Count}.\n" +
              $"[RESULT] One solution: {BestPairs[0].Item1} <=> {BestPairs[0].Item2}.\n";

        public string Solution => String.Join('\n', BestPairs.Select(x => $"{x.Item1} <=> {x.Item2}")) + "\n";
        public void Execute() {
            _Distance = _Algorithm.Solve(_PointSet);
        }
    }
}