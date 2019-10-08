using System.Numerics;

namespace ConsoleDriver {
    public class FibonacciExecutor:ConsoleDriver.Experiment.IExecutor {
        private Core.Fibonacci _Algorithm;
        public string Name => _Algorithm.AlgorithmName;

        private BigInteger _Value;
        public BigInteger Value => _Value;

        private BigInteger _N;
        public BigInteger Size => _N;
        public string Result => $"[RESULT] The No:{_N} Fibonacci number is: {_Value}.\n";
        public string Solution => $"{_Value.ToString()}\n";
        public void Execute() {
            _Value = _Algorithm.Solve(_N);
        }

        public FibonacciExecutor(Core.Fibonacci algorithm, BigInteger n) {
            _Algorithm = algorithm;
            _N = n;
        }
        
    }
}