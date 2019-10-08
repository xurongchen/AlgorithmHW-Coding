using System;
using System.IO;
using System.Numerics;
using System.Threading;

namespace ConsoleDriver {
    class ConsoleCall {
        static void Main(string[] args) {
            var sw = new StreamWriter(Console.OpenStandardOutput()) {AutoFlush = true};
            Console.SetOut(sw);
            Console.WriteLine("Input a number:");
            var numStr = Console.ReadLine();
            BigInteger num;
            if (BigInteger.TryParse(numStr,out num)) {
                var experimentE =
                    new Experiment(new FibonacciExecutor(new Core.FibonacciEfficient(), num),logStreamWriter: sw);
                experimentE.Start();
                while(experimentE.Now == Experiment.State.Running) Thread.Sleep(500);
                return;
            }
            Console.WriteLine("[Error] Please input a number.");
        }
    }
}