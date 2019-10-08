using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using ConsoleDriver;
using Core;

namespace ComparativeExperiment {
    class Program {
        private static int Round = 1;
        private static int SleepGap = 1000;
        static void Main(string[] args) {
            var Scales = new List<BigInteger>() {
                BigInteger.Parse("1"),
                BigInteger.Parse("10"),
                BigInteger.Parse("100"),
                BigInteger.Parse("1000"),
                BigInteger.Parse("10000"),
                BigInteger.Parse("100000"),
                BigInteger.Parse("1000000"),
                BigInteger.Parse("10000000"),
                BigInteger.Parse("100000000"),
                BigInteger.Parse("1000000000"),
                BigInteger.Parse("10000000000")
            };
            var sw = new StreamWriter(Console.OpenStandardOutput()) {AutoFlush = true};
            Console.SetOut(sw);
            foreach (var scale in Scales) {
                Console.WriteLine($"#Scale: {scale}");
                for (var round = 1; round <= Round; round++) {
                    Console.WriteLine($"##round: {round}");

                    Console.WriteLine($"Efficient algorithm");
                    var experimentE =
                        new Experiment(new FibonacciExecutor(new FibonacciEfficient(), scale),
                            logStreamWriter: sw);
                    experimentE.SetTimeout(60);
                    experimentE.Start();
                    while (experimentE.Now == Experiment.State.Running) {
                        Thread.Sleep(500);
                    }
                    Thread.Sleep(SleepGap);
                    Console.WriteLine($"Naive algorithm");
                    var experimentN =
                        new Experiment(new FibonacciExecutor(new FibonacciNaive(), scale),
                            logStreamWriter: sw);
                    experimentN.SetTimeout(60);
                    experimentN.Start();
                    while (experimentN.Now == Experiment.State.Running) {
                        Thread.Sleep(500);
                    }
                    Thread.Sleep(SleepGap);
                }
            }
        }
    }
}