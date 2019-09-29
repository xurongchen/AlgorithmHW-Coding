using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public abstract class ClosestPairOfPoints {
        public abstract List<Tuple<Point, Point>> BestPairs { get; }
        public abstract string AlgorithmName { get; }
        public abstract double Solve(IEnumerable<Point> PointSet);
    }
    
    public class ClosestPairOfPointsLinear: ClosestPairOfPoints {
        private readonly List<Tuple<Point, Point>> _BestPairs = new List<Tuple<Point, Point>>(); // Maybe more than one solution
        private HashSet<Tuple<Point, Point>> _MeetPair = new HashSet<Tuple<Point, Point>>();
        public override List<Tuple<Point, Point>> BestPairs => _BestPairs;
        public override string AlgorithmName => "Linear time closest pair of points";

        public override double Solve(IEnumerable<Point> PointSet) {
            _BestPairs.Clear();
            var SortedList = PointSet.OrderBy(item => item.y).ThenBy(item => item.x).ToList();
            SolveSorted(SortedList);
            return bestDistance;
        }
        private double bestDistance = Double.PositiveInfinity;

        private void SolveSorted(List<Point> PointsList) {
            if (PointsList.Count <= 1) return;
            int divideSize = PointsList.Count / 2;
            var divideComparer = KthLargestElement<Point>.GetKth(PointsList, divideSize + 1, Point.XOrder);
            SolveSorted(PointsList.FindAll(x => Point.XOrder(new Tuple<Point, Point>(x, divideComparer))));
            SolveSorted(PointsList.FindAll(x => !Point.XOrder(new Tuple<Point, Point>(x, divideComparer))));
            var ConsideredPoints = PointsList.FindAll(x => IsXInDistance(x, divideComparer, bestDistance));
            for (var i = 0; i < ConsideredPoints.Count; ++i) {
                for (var j = i + 1; j < ConsideredPoints.Count; ++j) {
                    if(_MeetPair.Contains(new Tuple<Point, Point>(ConsideredPoints[i], ConsideredPoints[j]))) continue; // Ensure each pair compare once
                    if(!IsYInDistance(ConsideredPoints[i],ConsideredPoints[j],bestDistance)) break; // Ensures iteration no more than 8
                    var newDistance = Point.Distance(ConsideredPoints[i], ConsideredPoints[j]);
                    if (Math.Abs(newDistance - bestDistance) < Double.Epsilon) {
                        _BestPairs.Add(new Tuple<Point, Point>(ConsideredPoints[i], ConsideredPoints[j]));
                    }
                    else if (newDistance < bestDistance) {
                        bestDistance = newDistance;
                        _BestPairs.Clear();
                        _BestPairs.Add(new Tuple<Point, Point>(ConsideredPoints[i], ConsideredPoints[j]));
                        _MeetPair.Add(new Tuple<Point, Point>(ConsideredPoints[i], ConsideredPoints[j]));
                    }
                }
            }
        }
        private bool IsXInDistance(Point p, Point cmp, double dst) => Point.XDistance(p,cmp) <= dst; // Consider equal to record multi-solutions
        private bool IsYInDistance(Point p, Point cmp, double dst) => Point.YDistance(p,cmp) <= dst; // Consider equal to record multi-solutions
    }

    public class ClosestPairOfPointsNaive: ClosestPairOfPoints {
        private readonly List<Tuple<Point, Point>> _BestPairs = new List<Tuple<Point, Point>>(); // Maybe more than one solution

        public override string AlgorithmName => "Naive closest pair of points";
        public override List<Tuple<Point, Point>> BestPairs => _BestPairs;
        public override double Solve(IEnumerable<Point> PointSet) {
            var PointList = PointSet.ToList();
            var Best = Double.PositiveInfinity;
            for (int i = 0; i < PointList.Count; ++i) {
                for (int j = i + 1; j < PointList.Count; ++j) {
                    var distance = Point.Distance(PointList[i], PointList[j]);
                    if (Math.Abs(distance - Best) < Double.Epsilon) {
                        _BestPairs.Add(new Tuple<Point, Point>(PointList[i], PointList[j]));
                    }
                    else if (distance < Best) {
                        _BestPairs.Clear();
                        _BestPairs.Add(new Tuple<Point, Point>(PointList[i], PointList[j]));
                        Best = distance;
                    }
                }
            }
            return Best;
        }
    }
}