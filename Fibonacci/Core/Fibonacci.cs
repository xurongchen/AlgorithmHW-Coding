using System;
using System.Numerics;

namespace Core {
    public abstract class Fibonacci {
        public abstract string AlgorithmName { get; }
        public abstract BigInteger Solve(BigInteger n);
        public override string ToString() => AlgorithmName;
    }

    public class FibonacciNaive: Fibonacci {
        public override string AlgorithmName => "Naive Fibonacci solver";
        
        public override BigInteger Solve(BigInteger n) {
            var fi = BigInteger.Zero;
            var fi1 = BigInteger.One;
            for (BigInteger i = 0; i < n; ++i) {
                BigInteger fi2 = fi + fi1;
                fi = fi1;
                fi1 = fi2;
            }
            return fi;
        }
    }

    public class FibonacciEfficient : Fibonacci {
        private static readonly BigIntegerMatrix2x2 _TransMatrix =
            new BigIntegerMatrix2x2(BigInteger.One, BigInteger.One, BigInteger.One, BigInteger.Zero);
        public override string AlgorithmName => "Efficient Fibonacci solver";
        public override BigInteger Solve(BigInteger n) {
            var powerMatrix = _TransMatrix ^ n;
            return powerMatrix.X10;
        }
    }
}

