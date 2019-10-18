using System;
using System.Collections.Generic;
using System.Drawing;
using Core;
using NDesk.Options;

namespace ConsoleDriver {
    class Program {
        static void Main(string[] args) {
            var show_help = false;

            var inputPath = "";
            var outputPath = "";
            var heightRate = 0.5;
            var widthRate = 0.5;
            
            var p = new OptionSet () {
                { "i|input=", "specific the input image.", v => { inputPath = v; } },
                { "o|output=", "specific the output image.", v => { outputPath = v; } },
                { "height", "specific the decreasing rate of height.\n" +
                            "(0.0~1.0, default: 0.5)",
                    (double v) => { heightRate = v; } },
                { "width", "specific the decreasing rate of width.\n" +
                           "(0.0~1.0, default: 0.5)", 
                    (double v) => { widthRate = v; } },
                { "h|?|help", "show help message.", v => { show_help = v != null; } },
            };
            
            List<string> extra;
            try {
                extra = p.Parse(args);
                if (show_help) {
                    ShowHelp(p);
                    return;
                }

                var bm = new Bitmap(inputPath, true);
                var sc = new SeamCarving(bm, Convert.ToInt32(bm.Height * heightRate),
                    Convert.ToInt32(bm.Width * widthRate));
                var bo = sc.Carve();
                bo.Save(outputPath);
            }
            catch (Exception e) {
                Console.Write("SeamCarving: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try 'SeamCarving --help' for more information.");
                return;
            }


//            var bm = new Bitmap(@"/Users/xrc/Repository/AlgorithmHW/SeamCarving/example3.jpg",true);
//            var sc = new SeamCarving(bm,bm.Height/2,bm.Width/2);
//            var bo = sc.Carve();
//            bo.Save(@"/Users/xrc/Repository/AlgorithmHW/SeamCarving/er3.jpg");
        }
        static void ShowHelp (OptionSet p)
        {
            Console.WriteLine ("Usage: SeamCarving [OPTIONS]");
            Console.WriteLine ("Run a seam carving algorithm to an image.");
            Console.WriteLine ();
            Console.WriteLine ("Options:");
            p.WriteOptionDescriptions (Console.Out);
        }

    }
}