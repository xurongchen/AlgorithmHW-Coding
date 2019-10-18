using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace Core {
    public abstract class EnergyAnalyzer {
        public MatrixInt32 matrixR, matrixB, matrixG;
        public abstract void Analyze();
        public abstract double GetEnergy(int px, int py);
        public void PreReAnalyze(List<Tuple<int, int>> removeList, bool isHorizontalRoute, bool Smoothing = true) {
            if (isHorizontalRoute) {
                var matrixRM = new MatrixInt32(matrixR.Row, matrixR.Col - 1);
                var matrixBM = new MatrixInt32(matrixR.Row, matrixR.Col - 1);
                var matrixGM = new MatrixInt32(matrixR.Row, matrixR.Col - 1);
                for (int r = 0; r < matrixRM.Row; ++r) {
                    for (int c = 0; c < matrixRM.Col; ++c) {
                        if (!Smoothing) {
                            matrixRM.Data[r, c] = c < removeList[r].Item2 ? matrixR.Data[r, c] : matrixR.Data[r, c + 1];
                            matrixBM.Data[r, c] = c < removeList[r].Item2 ? matrixB.Data[r, c] : matrixB.Data[r, c + 1];
                            matrixGM.Data[r, c] = c < removeList[r].Item2 ? matrixG.Data[r, c] : matrixG.Data[r, c + 1];
                        }
                        else {
                            if (c < removeList[r].Item2 - 1) {
                                matrixRM.Data[r, c] = matrixR.Data[r, c];
                                matrixBM.Data[r, c] = matrixB.Data[r, c];
                                matrixGM.Data[r, c] = matrixG.Data[r, c];
                            }
                            else if (c == removeList[r].Item2 - 1) {
                                matrixRM.Data[r, c] = (2 * matrixR.Data[r, c] + matrixR.Data[r, c + 1]) / 3;
                                matrixBM.Data[r, c] = (2 * matrixB.Data[r, c] + matrixB.Data[r, c + 1]) / 3;
                                matrixGM.Data[r, c] = (2 * matrixG.Data[r, c] + matrixG.Data[r, c + 1]) / 3;
                            }
                            else if (c == removeList[r].Item2){
                                matrixRM.Data[r, c] = (matrixR.Data[r, c] + 2 * matrixR.Data[r, c + 1]) / 3;
                                matrixBM.Data[r, c] = (matrixB.Data[r, c] + 2 * matrixB.Data[r, c + 1]) / 3;
                                matrixGM.Data[r, c] = (matrixG.Data[r, c] + 2 * matrixG.Data[r, c + 1]) / 3;
                            }
                            else {
                                matrixRM.Data[r, c] = matrixR.Data[r, c + 1];
                                matrixBM.Data[r, c] = matrixB.Data[r, c + 1];
                                matrixGM.Data[r, c] = matrixG.Data[r, c + 1];
                            }
                        }
                    }
                }
                matrixR = matrixRM;
                matrixB = matrixBM;
                matrixG = matrixGM;
            }
            else {
                var matrixRM = new MatrixInt32(matrixR.Row - 1, matrixR.Col);
                var matrixBM = new MatrixInt32(matrixR.Row - 1, matrixR.Col);
                var matrixGM = new MatrixInt32(matrixR.Row - 1, matrixR.Col);
                for (int c = 0; c < matrixRM.Col; ++c) {
                    for (int r = 0; r < matrixRM.Row; ++r) {
                        if (!Smoothing) {
                            matrixRM.Data[r, c] = r < removeList[c].Item1 ? matrixR.Data[r, c] : matrixR.Data[r + 1, c];
                            matrixBM.Data[r, c] = r < removeList[c].Item1 ? matrixB.Data[r, c] : matrixB.Data[r + 1, c];
                            matrixGM.Data[r, c] = r < removeList[c].Item1 ? matrixG.Data[r, c] : matrixG.Data[r + 1, c];
                        }
                        else {
                            if (r < removeList[c].Item1 - 1) {
                                matrixRM.Data[r, c] = matrixR.Data[r, c];
                                matrixBM.Data[r, c] = matrixB.Data[r, c];
                                matrixGM.Data[r, c] = matrixG.Data[r, c];
                            }
                            else if (r == removeList[c].Item1 - 1){
                                matrixRM.Data[r, c] = (matrixR.Data[r + 1, c] + 2 * matrixR.Data[r, c]) / 3;
                                matrixBM.Data[r, c] = (matrixB.Data[r + 1, c] + 2 * matrixB.Data[r, c]) / 3;
                                matrixGM.Data[r, c] = (matrixG.Data[r + 1, c] + 2 * matrixG.Data[r, c]) / 3;
                            }
                            else if (r == removeList[c].Item1){
                                matrixRM.Data[r, c] = (2 * matrixR.Data[r + 1, c] + matrixR.Data[r, c]) / 3;
                                matrixBM.Data[r, c] = (2 * matrixB.Data[r + 1, c] + matrixB.Data[r, c]) / 3;
                                matrixGM.Data[r, c] = (2 * matrixG.Data[r + 1, c] + matrixG.Data[r, c]) / 3;
                            }
                            else {
                                matrixRM.Data[r, c] = matrixR.Data[r + 1, c];
                                matrixBM.Data[r, c] = matrixB.Data[r + 1, c];
                                matrixGM.Data[r, c] = matrixG.Data[r + 1, c];
                            }
                        }
                    }
                }
                matrixR = matrixRM;
                matrixB = matrixBM;
                matrixG = matrixGM;
            }
        }
        
        public Bitmap BitmapGenerate() {
            var newBitmap = new Bitmap(matrixB.Row,matrixB.Col);
            for (int r = 0; r < newBitmap.Width; ++r) {
                for (int c = 0; c < newBitmap.Height; ++c) {
                    var color = new Color();
                    newBitmap.SetPixel(r, c,
                        Color.FromArgb(matrixR.Data[r, c], matrixG.Data[r, c], matrixB.Data[r, c]));
                }
            }
            return newBitmap;
        }
        public EnergyAnalyzer(Bitmap _Image) {
            matrixR = new MatrixInt32(_Image.Width,_Image.Height);
            matrixB = new MatrixInt32(_Image.Width,_Image.Height);
            matrixG = new MatrixInt32(_Image.Width,_Image.Height);
            for (int x = 0; x < _Image.Width; ++x) {
                for (int y = 0; y < _Image.Height; ++y) {
                    var color = _Image.GetPixel(x, y);
                    matrixR.Data[x,y] = color.R;
                    matrixB.Data[x,y] = color.B;
                    matrixG.Data[x,y] = color.G;
                }
            }
        }
    }    
    public interface RouteFinder {
        List<Tuple<int, int>> GetHorizontalRoute();
        List<Tuple<int, int>> GetVerticalRoute();
    }
    public class SeamCarving {
        private Bitmap _Image;
        public int Height => _Image.Height;
        public int Width => _Image.Width;
        
        private int _TargetHeight, _TargetWidth;

        public int TargetHeight {
            get => _TargetHeight;
            set => _TargetHeight = value;
        }

        public int TargetWidth {
            get => _TargetWidth;
            set => _TargetWidth = value;
        }

        public SeamCarving(Bitmap image, int targetHeight, int targetWidth) {
            _Image = image;
            _TargetHeight = targetHeight;
            _TargetWidth = targetWidth;
        }
        private EnergyAnalyzer _EnergyAnalyzer;
        public Bitmap Carve() {
            Console.WriteLine($"From {Width}x{Height} to {_TargetWidth}x{_TargetHeight}");
            var ea = new GradientEnergyAnalyzer(_Image);
            for (int nowHeight = Height; nowHeight > _TargetHeight; --nowHeight) {
                ea.Analyze();
                var finder = new BasicRouteFinder(ea);
                var route = finder.GetHorizontalRoute();
                ea.PreReAnalyze(route,true);
                Console.WriteLine($"Now size: {Width}x{nowHeight}");
            }
            for (int nowWidth = Width; nowWidth > _TargetWidth; --nowWidth) {
                ea.Analyze();
                var finder = new BasicRouteFinder(ea);
                var route = finder.GetVerticalRoute();
                ea.PreReAnalyze(route,false);
                Console.WriteLine($"Now size: {nowWidth}x{_TargetHeight}");
            }
            
            return ea.BitmapGenerate();
        }
    }

    public class GradientEnergyAnalyzer : EnergyAnalyzer {
        private readonly MatrixInt32 ConvolutionCore = new MatrixInt32(3, 3, new int[] 
        {
            1, 2, 1, 
            0, 0, 0, 
            -1, -2, -1
        });

        private MatrixInt32 AnalyzeResult;
        public override void Analyze() {
            var matrixSum = matrixR + matrixB + matrixG;
            MatrixInt32 result_tR = null,
                result_tRT = null,
                result_tB = null,
                result_tBT = null,
                result_tG = null,
                result_tGT = null;
            
            Thread tR = new Thread( () =>  result_tR = MatrixInt32.Abs(matrixR.Convolution(ConvolutionCore, 
                MatrixInt32.ConvolutionChoice.Same_FillZero)));
            Thread tRT = new Thread( () =>  result_tRT = MatrixInt32.Abs(matrixR.Convolution(ConvolutionCore.T,
                MatrixInt32.ConvolutionChoice.Same_FillZero)));
            Thread tB = new Thread( () =>  result_tB = MatrixInt32.Abs(matrixR.Convolution(ConvolutionCore, 
                MatrixInt32.ConvolutionChoice.Same_FillZero)));
            Thread tBT = new Thread( () =>  result_tBT = MatrixInt32.Abs(matrixR.Convolution(ConvolutionCore.T,
                MatrixInt32.ConvolutionChoice.Same_FillZero)));
            Thread tG = new Thread( () =>  result_tG = MatrixInt32.Abs(matrixR.Convolution(ConvolutionCore, 
                MatrixInt32.ConvolutionChoice.Same_FillZero)));
            Thread tGT = new Thread( () =>  result_tGT = MatrixInt32.Abs(matrixR.Convolution(ConvolutionCore.T,
                MatrixInt32.ConvolutionChoice.Same_FillZero)));
            tR.Start();
            tG.Start();
            tB.Start();
            tRT.Start();
            tGT.Start();
            tBT.Start();
            
            tR.Join();
            tG.Join();
            tB.Join();
            tRT.Join();
            tGT.Join();
            tBT.Join();
            MatrixInt32 sumR = null, sumB = null, sumG = null;
            Thread sR = new Thread(() => sumR = result_tR + result_tRT);
            Thread sB = new Thread(() => sumB = result_tB + result_tBT);
            Thread sG = new Thread(() => sumG = result_tG + result_tGT);
            sR.Start();
            sB.Start();
            sG.Start();
            sB.Join();
            sR.Join();
            sG.Join();
            AnalyzeResult = sumR + sumB + sumG;
//            AnalyzeResult =
//                MatrixInt32.Abs(matrixSum.Convolution(ConvolutionCore, 
//                    MatrixInt32.ConvolutionChoice.Same_FillZero))
//                + MatrixInt32.Abs(matrixSum.Convolution(ConvolutionCore.T,
//                    MatrixInt32.ConvolutionChoice.Same_FillZero));
        }

        public override double GetEnergy(int px, int py) => AnalyzeResult.Data[px, py];

        public GradientEnergyAnalyzer(Bitmap _Image):base(_Image) {}
    }

    public class BasicRouteFinder:RouteFinder {
        private EnergyAnalyzer _EnergyAnalyzer;
        private int _Width => _EnergyAnalyzer.matrixB.Row;
        private int _Height => _EnergyAnalyzer.matrixB.Col;

        public BasicRouteFinder(EnergyAnalyzer energyAnalyzer) {
            _EnergyAnalyzer = energyAnalyzer;
        }

        enum WhereFrom {
            Former, Current, Latter, Final
        }
        public List<Tuple<int, int>> GetHorizontalRoute() {
            var DP = new double [_Width,_Height];
            var Source = new WhereFrom [_Width,_Height];
            for (int y = 0; y < _Height; ++y) {
                DP[0, y] = _EnergyAnalyzer.GetEnergy(0, y);
                Source[0, y] = WhereFrom.Final;
            }
            for (int x = 1; x < _Width; ++x) {
                for (int y = 0; y < _Height; ++y) {
                    DP[x, y] = DP[x - 1, y];
                    Source[x, y] = WhereFrom.Current;
                    if (y >= 1 && DP[x, y] > DP[x - 1, y - 1]) {
                        DP[x, y] = DP[x - 1, y - 1];
                        Source[x, y] = WhereFrom.Former;
                    }
                    if (y < _Height - 1 && DP[x, y] > DP[x - 1, y + 1])  {
                        DP[x, y] = DP[x - 1, y + 1];
                        Source[x, y] = WhereFrom.Latter;
                    }
                    DP[x, y] += _EnergyAnalyzer.GetEnergy(x, y);
                }
            }

            int BestPos = 0;
            for (int y = 1; y < _Height; ++y) {
                if (DP[_Width - 1, y] < DP[_Width - 1, BestPos]) {
                    BestPos = y;
                }
            }

            int level = _Width - 1;
            var result = new List<Tuple<int, int>>();
            while (Source[level, BestPos] != WhereFrom.Final) {
                result.Add(new Tuple<int, int>(level, BestPos));
                BestPos = Source[level, BestPos] switch {
                    WhereFrom.Current => BestPos,
                    WhereFrom.Former => BestPos - 1,
                    WhereFrom.Latter => BestPos + 1,
                    _ => throw new NotSupportedException()
                };
                --level;
            }
            result.Add(new Tuple<int, int>(level, BestPos));;
            result.Reverse();
            return result;
        }

        public List<Tuple<int, int>> GetVerticalRoute() {
            var DP = new double [_Width,_Height];
            var Source = new WhereFrom [_Width,_Height];
            for (int x = 0; x < _Width; ++x) {
                DP[x, 0] = _EnergyAnalyzer.GetEnergy(x, 0);
                Source[x, 0] = WhereFrom.Final;
            }
            for (int y = 1; y < _Height; ++y) {
                for (int x = 0; x < _Width; ++x) {
                    DP[x, y] = DP[x, y - 1];
                    Source[x, y] = WhereFrom.Current;
                    if (x >= 1 && DP[x, y] > DP[x - 1, y - 1]) {
                        DP[x, y] = DP[x - 1, y - 1];
                        Source[x, y] = WhereFrom.Former;
                    }
                    if (x < _Width - 1 && DP[x, y] > DP[x + 1, y - 1])  {
                        DP[x, y] = DP[x + 1, y - 1];
                        Source[x, y] = WhereFrom.Latter;
                    }
                    DP[x, y] += _EnergyAnalyzer.GetEnergy(x, y);
                }
            }

            int BestPos = 0;
            for (int x = 1; x < _Width; ++x) {
                if (DP[x, _Height - 1] < DP[BestPos, _Height - 1]) {
                    BestPos = x;
                }
            }

            int level = _Height - 1;
            var result = new List<Tuple<int, int>>();
            while (Source[BestPos, level] != WhereFrom.Final) {
                result.Add(new Tuple<int, int>(BestPos, level));
                BestPos = Source[BestPos, level] switch {
                    WhereFrom.Current => BestPos,
                    WhereFrom.Former => BestPos - 1,
                    WhereFrom.Latter => BestPos + 1,
                    _ => throw new NotSupportedException()
                };
                --level;
            }
            result.Add(new Tuple<int, int>(BestPos, level));
            result.Reverse();
            return result;
        }
    }
}