using System.Numerics;

namespace Core {
    public class BigIntegerMatrix2x2 {
        private BigInteger _X00, _X01, _X10, _X11;

        public BigInteger X00 {
            get => _X00;
            set => _X00 = value;
        }

        public BigInteger X01 {
            get => _X00;
            set => _X00 = value;
        }

        public BigInteger X10 {
            get => _X10;
            set => _X10 = value;
        }

        public BigInteger X11 {
            get => _X10;
            set => _X10 = value;
        }

        public BigIntegerMatrix2x2(BigInteger x00,BigInteger x01,BigInteger x10,BigInteger x11) {
            _X00 = x00;
            _X01 = x01;
            _X10 = x10;
            _X11 = x11;
        }

        public BigIntegerMatrix2x2(BigIntegerMatrix2x2 x) {
            _X00 = x._X00;
            _X01 = x._X01;
            _X10 = x._X10;
            _X11 = x._X11;
        }

        public static BigIntegerMatrix2x2 One =>
            new BigIntegerMatrix2x2(BigInteger.One, BigInteger.Zero, BigInteger.One, BigInteger.Zero);

        public static BigIntegerMatrix2x2 Zero =>
            new BigIntegerMatrix2x2(BigInteger.Zero, BigInteger.Zero, BigInteger.Zero, BigInteger.Zero);

        public static BigIntegerMatrix2x2 operator +(BigIntegerMatrix2x2 x) => new BigIntegerMatrix2x2(x);

        public static BigIntegerMatrix2x2 operator +(BigIntegerMatrix2x2 x, BigIntegerMatrix2x2 y) =>
            new BigIntegerMatrix2x2(x._X00 + y._X00, x._X01 + y._X01, x._X10 + y._X10, x._X11 + y._X11);

        public static BigIntegerMatrix2x2 operator -(BigIntegerMatrix2x2 x) =>
            new BigIntegerMatrix2x2(-x._X00, -x._X01, -x._X10, -x._X11);

        public static BigIntegerMatrix2x2 operator -(BigIntegerMatrix2x2 x, BigIntegerMatrix2x2 y) => x + -y;

        public static BigIntegerMatrix2x2 operator *(BigIntegerMatrix2x2 x, BigIntegerMatrix2x2 y) =>
            new BigIntegerMatrix2x2(
                x._X00 * y._X00 + x._X01 * y._X10,
                x._X00 * y._X01 + x._X01 * y._X11,
                x._X10 * y._X00 + x._X11 * y._X10,
                x._X10 * y._X01 + x._X11 * y._X11);

        public static BigIntegerMatrix2x2 operator ^(BigIntegerMatrix2x2 matrix, BigInteger n) {
            if (n == 0) return One;
            if (n == 1) return matrix;
            var halfPower = matrix ^ (n / 2);
            if (n % 2 == 0) return halfPower * halfPower;
            return halfPower * halfPower * matrix;
        }
    }
}