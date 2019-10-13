using System;
using System.Drawing;
using Core;
namespace ConsoleDriver {
    class Program {
        static void Main(string[] args) {
            var bm = new Bitmap(@"/Users/xrc/Repository/AlgorithmHW/SeamCarving/example3.jpg",true);
            var sc = new SeamCarving(bm,bm.Height/2,bm.Width/2);
            var bo = sc.Carve();
            bo.Save(@"/Users/xrc/Repository/AlgorithmHW/SeamCarving/er3.jpg");
        }
    }
}