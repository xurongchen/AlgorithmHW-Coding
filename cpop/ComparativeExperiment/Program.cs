using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using ConsoleDriver;
using Core;

namespace ComparativeExperiment {
    class Program {
        static void Main(string[] args) {
            var Scales = new List<int>() {
                100,
                500,
                1000,
                5000,
                10000,
                50000,
                100000,
                500000,
                1000000
            };
            var sw = new StreamWriter(Console.OpenStandardOutput()) {AutoFlush = true};
            Console.SetOut(sw);
            foreach (var scale in Scales) {
                Console.WriteLine($"#Scale: {scale}");
                for (var round = 1; round <= 3; round++) {
                    Console.WriteLine($"##round: {round}");
                    var PointSetE = Point.RandomPoints(-100, 100, scale).ToList();
                    var PointSetN = new List<Point>(PointSetE);
                    Console.WriteLine($"Efficient algorithm");
                    var experimentE =
                        new Experiment(new ClosestPairOfPointsExecutor(new ClosestPairOfPointsEfficient(), PointSetE),
                            logStreamWriter: sw);
                    experimentE.SetTimeout(60);
                    experimentE.Start();
                    while (experimentE.Now == Experiment.State.Running) {
                        Thread.Sleep(500);
                    }
                    Thread.Sleep(10000);
                    Console.WriteLine($"Naive algorithm");
                    var experimentN =
                        new Experiment(new ClosestPairOfPointsExecutor(new ClosestPairOfPointsNaive(), PointSetN),
                            logStreamWriter: sw);
                    experimentN.SetTimeout(60);
                    experimentN.Start();
                    while (experimentN.Now == Experiment.State.Running) {
                        Thread.Sleep(500);
                    }
                    Thread.Sleep(10000);
                }
            }
        }
    }
}