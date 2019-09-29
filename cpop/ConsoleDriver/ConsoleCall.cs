using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace ConsoleDriver {
    static class ConsoleCall {
        private static double UpperBound = Double.NaN, LowerBound = Double.NaN;
        private static IEnumerable<Core.Point> PointSet = null;
        private static double lastResult = Double.NaN;
        private static List<Tuple<Core.Point, Core.Point>> lastBestPairs = null;
        private static string AlgorithmName = null;
        public static int Timeout = 60000; // ms
        static void Main(string[] args) {
            int option = -2;
            while (true) {
                string inputString = "";
                if(option != -2) {
                    Console.WriteLine("Enter to continue...");
                    Console.ReadLine();
                }
                Console.Clear();
                Console.Write(MainInterface);
                inputString = Console.ReadLine();
                if (!Int32.TryParse(inputString, out option)) {
                    option = -1;
                    Console.Write(OptionError);
                    continue;
                }
                System.Diagnostics.Stopwatch stopwatch = null;
                switch (option) {
                    case 0: // Print values now
                        if(Double.IsNaN(UpperBound) || Double.IsNaN(LowerBound)) Console.Write(BoundNoSet);
                        else {
                            Console.Write(BoundInformation());
                        } 
                        if(PointSet == null) Console.Write(PointSetEmpty);
                        else {
                            Console.Write(PointSetInformation());
                        }
                        if(Double.IsNaN(lastResult) || lastBestPairs == null) Console.Write(NoExecution);
                        else {
                            Console.Write(ExecuteInformation());
                        }
                        break;
                    case 1: // Set bounds
                        Console.Write(LowerBoundText);
                        inputString = Console.ReadLine();
                        if (!Double.TryParse(inputString, out var NewLowerBound)) {
                            Console.Write(BoundFormatError);
                            break;
                        }
                        Console.Write(UpperBoundText);
                        inputString = Console.ReadLine();
                        if (!Double.TryParse(inputString, out var NewUpperBound)) {
                            Console.Write(BoundFormatError);
                            break;
                        }
                        if (NewUpperBound <= NewLowerBound) {
                            Console.Write(BoundLogicalError);
                            break;
                        }
                        UpperBound = NewUpperBound;
                        LowerBound = NewLowerBound;
                        break;
                    case 2: // Random simply
                        if(Double.IsNaN(UpperBound) || Double.IsNaN(LowerBound)) {
                            Console.Write(BoundNoSetError);
                            break;
                        }
                        Console.Write(PointSizeText);
                        inputString = Console.ReadLine();
                        if (!Int32.TryParse(inputString, out var pointSize)) {
                            Console.Write(SizeFormatError);
                            break;
                        }
                        if (pointSize <= 1) {
                            Console.Write(SizeLogicalError);
                            break;
                        }
                        PointSet = Core.Point.RandomPoints(LowerBound, UpperBound, pointSize).ToList();
                        Console.Write(PointSetInformation());
                        break;
                    case 3: // Execute linear time algorithm
                        stopwatch = new System.Diagnostics.Stopwatch();
                        stopwatch.Start();
                        if (!RunClosestPairOfPoints()) {
                            stopwatch.Stop();
                            Console.Write(PointSetError);
                            break;
                        }
                        stopwatch.Stop();
                        Console.Write(ExecuteSuccess);
                        Console.Write(TimeUse(stopwatch));
                        Console.Write(ExecuteInformation());
                        break;
                    case 4: // Execute naive algorithm
                        stopwatch = new System.Diagnostics.Stopwatch();
                        stopwatch.Start();
                        if (!RunClosestPairOfPoints(naive:true)) {
                            stopwatch.Stop();
                            Console.Write(PointSetError);
                            break;
                        }
                        stopwatch.Stop();
                        Console.Write(ExecuteSuccess);
                        Console.Write(TimeUse(stopwatch));
                        Console.Write(ExecuteInformation());
                        break;
                    case 8: // About
                        Console.Write(Me);
                        break;
                    case 9: // Exit
                        Console.Write(Bye);
                        return;
                    default:
                        Console.Write(OptionUnknown);
                        option = -1;
                        break;
                }
            }
        }

        private static bool RunClosestPairOfPoints(bool naive = false) {
            if (PointSet == null) return false;
            var executor = naive
                ? (ClosestPairOfPoints) new Core.ClosestPairOfPointsNaive()
                : new Core.ClosestPairOfPointsLinear();
            lastResult = executor.Solve(PointSet);
            lastBestPairs = executor.BestPairs;
            AlgorithmName = executor.AlgorithmName;
            return true;
        }

        private static void WritePointSet(string filePath) {
            var file = new System.IO.StreamWriter(filePath);
            foreach (var point in PointSet)
            {
                file.WriteLine(point);
            }
            file.Close();
        }
        private static void WriteSolution(string filePath) {
            var file = new System.IO.StreamWriter(filePath);
            foreach (var pointPair in lastBestPairs)
            {
                file.WriteLine($"{pointPair.Item1} <=> {pointPair.Item2}");
            }
            file.Close();
        }
        

        private static readonly string MainInterface = $"Input option number:\n" +
                                                       $"[0] Print logs\n" +
                                                       $"[1] Set bounds\n" +
                                                       $"[2] Random a point set\n" +
                                                       $"[3] Execute linear time algorithm\n" +
                                                       $"[4] Execute naive algorithm\n" +
                                                       $"[8] About\n" +
                                                       $"[9] Exit\n";

        private static readonly string BoundNoSet = $"The bound is not set yet.\n";
        private static readonly string PointSetEmpty = $"The point set is empty yet.\n";
        private static readonly string NoExecution = $"Execution is not done yet.\n";

        private static readonly string OptionError = $"[ERROR] Format the option failed!\n";
        private static readonly string OptionUnknown = $"[ERROR] Unknown option is given!\n";

        private static readonly string SizeFormatError = $"[ERROR] Format the size failed!\n";
        private static readonly string SizeLogicalError = $"[ERROR] The size should be larger than 1!\n";
        private static readonly string TimeoutFormatError = $"[ERROR] Format the time failed!\n";
        private static readonly string TimeoutLogicalError = $"[ERROR] The size should be in [1,86400]\n";

        
        private static readonly string BoundNoSetError = $"[ERROR] The bound should be set before random.\n";
        private static readonly string PointSizeText = $"Please input the size of point set (int):\n";
        private static readonly string TimeoutText = $"Please input the bound of timeout [default 60s] (int):\n";
        private static readonly string ExecuteSuccess = $"[OK] We find the closest pair of points.\n";
        private static readonly string PointSetError = $"[ERROR] Point set has not been initialized!\n";

        private static readonly string UpperBoundText = $"Please input the upper bound (double):\n";
        private static readonly string LowerBoundText = $"Please input the lower bound (double):\n";
        private static readonly string BoundFormatError = $"[ERROR] Format the bound failed!\n";

        private static readonly string BoundLogicalError =
            $"[ERROR] The upper bound should be larger than the lower bound!\n";

        private static readonly string Bye = $"Bye~\n";

        private static readonly string Me = $"By: Xu Rongchen   ID: 2019214528\n";

        public static string ExecuteInformation(string SolutionPath=@"Solution.txt") {
            var ret = AlgorithmName + $":\n";
            if (Double.IsPositiveInfinity(lastResult)) {
                ret += $"Closest distance: INF.(Single point)\n";
            }
            else {
                ret += $"Closest distance: {lastResult}.\n";
                ret += $"Solution count: {lastBestPairs.Count}. See them in {SolutionPath}\n";
                ret += $"Solution example: {lastBestPairs[0].Item1} <=> {lastBestPairs[0].Item2}.\n";
                WriteSolution(SolutionPath);
            }
            return ret;
        }

        public static string PointSetInformation(string PointSetPath=@"PointSet.txt") {
            var ret = $"Point set has {PointSet.ToList().Count} points. See them in {PointSetPath}\n";
            WritePointSet(PointSetPath);
            return ret;
        }

        public static string BoundInformation() {
            return $"Bound setting: LowerBound: {LowerBound}, UpperBound: {UpperBound}\n";
        }

        public static string TimeUse(System.Diagnostics.Stopwatch stopwatch) {
            return $"Total time use: {stopwatch.Elapsed}\n";
        }

        public static string TimeoutSetting() {
            return $"Timeout setting: {Timeout / 1000}s\n";
        }
    }
}

