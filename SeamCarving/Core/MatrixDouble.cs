using System;
using System.Linq;

namespace Core {
    public class MatrixDouble {
        protected int _Col, _Row;
        public int Col => _Col;
        public int Row => _Row;
        protected double[,] _Data;

        public double[,] Data {
            get => _Data;
            set => _Data = value;
        }

        public MatrixDouble(int row, int col, double[] data = null) {
            _Col = col;
            _Row = row;
            _Data = new double[_Row,_Col];
            if (data != null) {
                if (data.Length != row * col) {
                    throw new ArgumentException("Matrix initialize error.");
                }
                for (int r = 0, dataPos = 0; r < _Row; ++r) {
                    for (int c = 0; c < _Col; ++c) {
                        _Data[r, c] = data[dataPos++];
                    }
                }
            }
        }
        public MatrixDouble(MatrixDouble matrix) {
            _Col = matrix._Col;
            _Row = matrix._Row;
            _Data = new double[_Row,_Col];
            Array.Copy(matrix._Data, _Data, _Row * _Col);
        }
        
        public MatrixDouble(MatrixInt32 matrix) {
            _Col = matrix.Col;
            _Row = matrix.Row;
            _Data = new double[_Row,_Col];
            for (int r = 0; r < _Row; ++r) {
                for (int c = 0; c < _Col; ++c) {
                    _Data[r, c] = matrix.Data[r, c];
                }
            }
        }
        public MatrixDouble Clone() => new MatrixDouble(this);
        
        public static MatrixDouble Zero(int row, int col) {
            var matrix =  new MatrixDouble(row,col);
            for (int r = 0; r < row; ++r) {
                for (int c = 0; c < col; ++c) {
                    matrix._Data[r, c] = 0;
                }
            }
            return matrix;
        }
        
        public static MatrixDouble Transpose(MatrixDouble x) {
            var matrix =  new MatrixDouble(x._Col,x._Row);
            for (int r = 0; r < x._Row; ++r) {
                for (int c = 0; c < x._Col; ++c) {
                    matrix._Data[c, r] = x._Data[r, c];
                }
            }
            return matrix;
        }
        public MatrixDouble T => Transpose(this);
        
        public static MatrixDouble operator +(MatrixDouble x) => new MatrixDouble(x);
        public static MatrixDouble operator -(MatrixDouble x) { 
            var matrix =  new MatrixDouble(x._Row, x._Col);
            for (int r = 0; r < x._Row; ++r) {
                for (int c = 0; c < x._Col; ++c) {
                    matrix._Data[r, c] = -x._Data[r, c];
                }
            }
            return matrix;
        }

        public static MatrixDouble operator +(MatrixDouble x, MatrixDouble y) {
            if (x._Col != y._Col || x._Row != y._Row) 
                throw new InvalidOperationException("Matrix size error.");
            var matrix =  new MatrixDouble(x._Row, x._Col);
            for (int r = 0; r < x._Row; ++r) {
                for (int c = 0; c < x._Col; ++c) {
                    matrix._Data[r, c] = x._Data[r, c] + y._Data[r, c];
                }
            }
            return matrix;
        }
        public static MatrixDouble operator -(MatrixDouble x, MatrixDouble y) {
            if (x._Col != y._Col || x._Row != y._Row) 
                throw new InvalidOperationException("Matrix size error.");
            var matrix =  new MatrixDouble(x._Row, x._Col);
            for (int r = 0; r < x._Row; ++r) {
                for (int c = 0; c < x._Col; ++c) {
                    matrix._Data[r, c] = x._Data[r, c] - y._Data[r, c];
                }
            }
            return matrix;
        }
        public static MatrixDouble operator *(MatrixDouble x, MatrixDouble y) {
            if (x._Col != y._Row) 
                throw new InvalidOperationException("Matrix size error.");
            var matrix =  new MatrixDouble(x._Row, x._Col);
            for (int r = 0; r < x._Row; ++r) {
                for (int c = 0; c < y._Col; ++c) {
                    matrix._Data[r, c] = 0;
                    for (int i = 0; i < x._Col; ++i) {
                        matrix._Data[r, c] += x._Data[r, i] * y._Data[i, c];
                    }
                }
            }
            return matrix;
        }
        
        public static MatrixDouble operator *(double C, MatrixDouble x) { 
            var matrix =  new MatrixDouble(x._Row, x._Col);
            for (int r = 0; r < x._Row; ++r) {
                for (int c = 0; c < x._Col; ++c) {
                    matrix._Data[r, c] = C * x._Data[r, c];
                }
            }
            return matrix;
        }

        public static MatrixDouble operator *(MatrixDouble x, double C) => C * x;

        public static MatrixDouble Abs(MatrixDouble x) {
            var matrix =  new MatrixDouble(x._Row, x._Col);
            for (int r = 0; r < x._Row; ++r) {
                for (int c = 0; c < x._Col; ++c) {
                    matrix.Data[r, c] = x._Data[r, c] >= 0 ? x._Data[r, c] : -x._Data[r, c];
                }
            }
            return matrix;
        }
        
        public static MatrixInt32 Int(MatrixDouble x) {
            var matrix =  new MatrixInt32(x._Row, x._Col);
            for (int r = 0; r < x._Row; ++r) {
                for (int c = 0; c < x._Col; ++c) {
                    matrix.Data[r, c] = (int) x._Data[r, c];
                }
            }
            return matrix;
        }
        
        public enum ConvolutionChoice {
            Valid,Same_FillZero,Same_EdgeCopy
        }
        private static bool AddUp = true;
        private static bool AddLeft = true;
        public MatrixDouble Convolution(MatrixDouble core, ConvolutionChoice choice, int step = 1) {
            MatrixDouble matrixH, matrixHV;
            if (choice == ConvolutionChoice.Same_EdgeCopy || choice == ConvolutionChoice.Same_FillZero) {
                if (step != 1) {
                    throw new NotSupportedException("Only support valid convolution when step is 1.");
                }
            }
            int US, DS, LS, RS;
            switch (choice) {
                case ConvolutionChoice.Valid:
                    break;
                case ConvolutionChoice.Same_FillZero:
                    US = (core._Row - 1) / 2;
                    DS = (core._Row - 1) / 2;
                    LS = (core._Col - 1) / 2;
                    RS = (core._Col - 1) / 2;
                    if (US * 2 != core._Row) {
                        if (AddUp) ++US;
                        else ++DS;
                        AddUp = !AddUp;
                    }
                    if (LS * 2 != core._Col) {
                        if (AddLeft) ++LS;
                        else ++RS;
                        AddLeft = !AddLeft;
                    }
                    matrixH = HorizontalConcatenate(Zero(_Row,LS),this,Zero(_Row,RS));
                    matrixHV = VerticalConcatenate(Zero(US, matrixH._Col), matrixH, Zero(DS, matrixH._Col));
                    return matrixHV.Convolution(core, ConvolutionChoice.Valid);
                    case ConvolutionChoice.Same_EdgeCopy:
                    US = (core._Row - 1) / 2;
                    DS = (core._Row - 1) / 2;
                    LS = (core._Col - 1) / 2;
                    RS = (core._Col - 1) / 2;
                    if (US * 2 != core._Row) {
                        if (AddUp) ++US;
                        else ++DS;
                        AddUp = !AddUp;
                    }
                    if (LS * 2 != core._Col) {
                        if (AddLeft) ++LS;
                        else ++RS;
                        AddLeft = !AddLeft;
                    }
                    var AddL = Enumerable.Repeat(GetCol(0), LS);
                    var AddR = Enumerable.Repeat(GetCol(-1), RS);
                    var HorizontalList = AddL.Append(this).Concat(AddR).ToArray();
                    matrixH = HorizontalConcatenate(HorizontalList);
                    var AddU = Enumerable.Repeat(GetRow(0), US);
                    var AddD = Enumerable.Repeat(GetCol(-1), DS);
                    var VerticalList = AddU.Append(matrixH).Concat(AddD).ToArray();
                    matrixHV = VerticalConcatenate(VerticalList);
                    return matrixHV.Convolution(core, ConvolutionChoice.Valid);
            }

            #region Calculate valid convolution

            var resultMatrix = Zero(1 + (_Row - core._Row)/step, 1 + (_Col - core._Col)/step);

            int XDelta = core._Row - 1, YDelta = core._Col - 1, XLen = core._Row, YLen = core._Col;
            
            for (int px = 0, sx = 0; px < resultMatrix._Row; ++px, sx+=step) {
                for (int py = 0, sy = 0; py < resultMatrix._Col; ++py, sy+=step) {
                    for (int x = 0; x < XLen; ++x) {
                        for (int y = 0; y < YLen; ++y) {
                            resultMatrix._Data[px, py] += _Data[sx + x, sy + y] * core._Data[XDelta - x, YDelta - y];
                        }
                    }
                }
            }
            #endregion

            return resultMatrix;
        }

        public static MatrixDouble HorizontalConcatenate(params MatrixDouble[] matrixArray) {
            var ColSum = 0;
            foreach (var m in matrixArray) {
                if (m._Row != matrixArray[0]._Row) {
                    throw new InvalidOperationException("Matrix size error.");
                }
                ColSum += m._Col;
            }
            var matrix = new MatrixDouble(matrixArray[0]._Row, ColSum);
            var ColPos = 0;
            foreach (var m in matrixArray) {
                for (int c = 0; c < m._Col; ++c,++ColPos) {
                    for (int r = 0; r < m._Row; ++r) {
                        matrix._Data[r, ColPos] = m._Data[r, c];
                    }
                }
            }
            return matrix;
        }
        public static MatrixDouble VerticalConcatenate(params MatrixDouble[] matrixArray) {
            var RowSum = 0;
            foreach (var m in matrixArray) {
                if (m._Col != matrixArray[0]._Col) {
                    throw new InvalidOperationException("Matrix size error.");
                }
                RowSum += m._Row;
            }
            var matrix = new MatrixDouble(RowSum, matrixArray[0]._Col);
            var RowPos = 0;
            foreach (var m in matrixArray) {
                for (int r = 0; r < m._Row; ++r,++RowPos) {
                    for (int c = 0; c < m._Col; ++c) {
                        matrix._Data[RowPos, c] = m._Data[r, c];
                    }
                }
            }
            return matrix;
        }

        public MatrixDouble GetRow(int row) => GetRow(row, row);
        public MatrixDouble GetRow(int begin, int end) {
            if (begin > end) {
                throw new ArgumentException("Begin position should not be large than end.");
            }
            if (begin < 0) begin = _Row + begin;
            if (end < 0) end = _Row + end;
            if (begin < 0 || begin >= _Row || end < 0 || end >= _Row) {
                throw new ArgumentException("Begin or end position exceed the range.");
            }
            
            var matrix = new MatrixDouble(end - begin + 1, _Col);
            for (int r = 0; r < matrix._Row; ++r) {
                for (int c = 0; c < matrix._Col; ++c) {
                    matrix._Data[r, c] = _Data[r + begin, c];
                }
            }

            return matrix;
        }
        
        public MatrixDouble GetCol(int row) => GetCol(row, row);
        public MatrixDouble GetCol(int begin, int end) {
            if (begin > end) {
                throw new ArgumentException("Begin position should not be large than end.");
            }
            if (begin < 0) begin = _Col + begin;
            if (end < 0) end = _Col + end;
            if (begin < 0 || begin >= _Col || end < 0 || end >= _Col) {
                throw new ArgumentException("Begin or end position exceed the range.");
            }

            var matrix = new MatrixDouble(_Row, end - begin + 1);
            for (int r = 0; r < matrix._Row; ++r) {
                for (int c = 0; c < matrix._Col; ++c) {
                    matrix._Data[r, c] = _Data[r, c + begin];
                }
            }

            return matrix;
        }
    }

    public class SquareMatrixDouble : MatrixDouble {
        public SquareMatrixDouble(int size, double[] data = null) : base(size, size, data: data) {}
        
        public static SizedMatrix Size(int size) => new SizedMatrix(size);
        public class SizedMatrix {
            private int _Size;
            public SizedMatrix(int size) {
                _Size = size;
            }
            public SquareMatrixDouble GetZero() {
                var matrix = new SquareMatrixDouble(_Size);
                for (int r = 0; r < _Size; ++r) {
                    for (int c = 0; c < _Size; ++c) {
                        matrix._Data[r, c] = 0;
                    }
                }
                return matrix;
            }
            public SquareMatrixDouble GetOne() {
                var matrix = GetZero();
                for (int i = 0; i < _Size; ++i) {
                    matrix._Data[i, i] = 1;
                }
                return matrix;
            }
        }
    }
}