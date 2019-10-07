using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleDriver;
using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebDriver.Models;

namespace WebDriver.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
        }

        private static ConsoleDriver.Experiment _experiment = null;
        private static List<Point> _pointSet = new List<Point>();
        
        public IActionResult Index() {
//            _pointSet.Add(new Point(0,0));
            ViewBag.PointSet = _pointSet;
            ViewBag.DataListStr = String.Join(',',_pointSet.Select(p => $"[{p.x},{p.y}]").ToList());
            return View();
        }

        [HttpPost]
        public IActionResult AddPoint(string px,string py) {
            double dx,dy; 
            if(Double.TryParse(px,out dx)&&Double.TryParse(py,out dy)) {
                _pointSet.Add(new Point(dx, dy));
                _experiment = null;
            }
            return RedirectToAction("Index");
        }

        public IActionResult DelPoint(int id) {
            foreach (var point in _pointSet) {
                if(point.id==id) {
                    _pointSet.Remove(point);
                    _experiment = null;
                    break;
                }
            }
            return RedirectToAction("Index");
        }
        
        public IActionResult Result() {
            ViewBag.Message = "";
            if (_experiment != null) {
                ViewBag.Message += _experiment.Now switch {
                    Experiment.State.Init => $"Experiment is Initializing...\n",
                    Experiment.State.Running => $"Experiment is still running...\n",
                    Experiment.State.Error => $"Experiment is timeout.\n",
                    Experiment.State.Finished => "",
                    _ => throw new Exception("No state found")
                };
                if (ViewBag.Message == "") {
                    ViewBag.DataListStr = String.Join(',',_pointSet.Select(p => $"[{p.x},{p.y}]").ToList());
                    if(_experiment.Executor is ClosestPairOfPointsExecutor cpopExec) {
                        ViewBag.Distance = cpopExec.Distance;
                        ViewBag.PointPairStr = $"[{cpopExec.BestPairs[0].Item1.x},{cpopExec.BestPairs[0].Item1.y}]," +
                                               $"[{cpopExec.BestPairs[0].Item2.x},{cpopExec.BestPairs[0].Item2.y}]";
                    }
                }
            }
            else {
                if (_pointSet.Count < 2) ViewBag.Message += $"Points less than 2!\n";
                else {
                    _experiment = new Experiment(
                        new ClosestPairOfPointsExecutor(new ClosestPairOfPointsEfficient(), _pointSet)
                        );
                    _experiment.Start();
                    return RedirectToAction("Result");
                }
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}