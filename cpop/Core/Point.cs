using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Core {
    public class Point {
        private static uint _nextId = 0;
        private readonly double _x, _y;
        private readonly uint _id;
        public uint id => _id;
        public double x => _x;
        public double y => _y;

        private static Random rand = null;

        public override string ToString() {
            return $"({x},{y})";
        }

        public Point(double x, double y) {
            _x = x;
            _y = y;
            _id = _nextId++;
        }
        public static Point RandomPoint(double lowerBound, double upperBound) {
            if(rand == null) rand = new Random();
            return new Point(rand.NextDouble() * (upperBound - lowerBound) + lowerBound,rand.NextDouble() * (upperBound - lowerBound) + lowerBound);
        }

        public static double Distance(Point p1, Point p2) => Math.Sqrt((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y));
        public static double XDistance(Point p1, Point p2) => Math.Abs(p1.x - p2.x);
        public static double YDistance(Point p1, Point p2) => Math.Abs(p1.y - p2.y);
        
        public static bool IDOrder(Tuple<Point,Point> t) => t.Item1._id < t.Item2._id;
        public static bool XOrder(Tuple<Point,Point> t) => Math.Abs(t.Item1.x - t.Item2.x) < Double.Epsilon ? t.Item1.y<t.Item2.y : t.Item1.x<t.Item2.x;
        public static bool YOrder(Tuple<Point,Point> t) => Math.Abs(t.Item1.y - t.Item2.y) < Double.Epsilon ? t.Item1.x<t.Item2.x : t.Item1.y<t.Item2.y;

        public static IEnumerable<Point> RandomPoints(double lowerBound, double upperBound, int size) => Enumerable.Range(0, size).Select(x => RandomPoint(lowerBound, upperBound));
        
    }
}