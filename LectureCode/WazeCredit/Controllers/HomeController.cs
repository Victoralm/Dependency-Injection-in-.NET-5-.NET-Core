using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WazeCredit.Models;
using WazeCredit.Models.ViewModels;
using WazeCredit.Services;

namespace WazeCredit.Controllers
{
    public class HomeController : Controller
    {
        public HomeVM HomeVM { get; set; }
        private readonly IMarketForecaster _marketForecaster;

        /// <summary>
        /// Injecting IMarketForecaster as a Dependency
        /// </summary>
        public HomeController(IMarketForecaster marketForecaster)
        {
            this._marketForecaster = marketForecaster;
            HomeVM = new HomeVM();
        }

        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            MarketResult currentMarket = this._marketForecaster.GetMarketPrediction();

            switch (currentMarket.MarketCondition)
            {
                case MarketCondition.StableUp:
                    HomeVM.MarketForecast = "Market shows signs to go up in a stable state! It is a great sign to apply for credit applications!";
                    break;
                case MarketCondition.StableDown:
                    HomeVM.MarketForecast = "Market shows signs to go down in a stable state! It is a not a good sign to apply for credit applications! But extra credit is always piece of mind if you have handy when you need it.";
                    break;
                case MarketCondition.Volatile:
                    HomeVM.MarketForecast = "Market shows signs of volatility. In uncertain times, it is good to have credit handy if you need extra funds!";
                    break;
                default:
                    HomeVM.MarketForecast = "Apply for a credit card using our application!";
                    break;
            }

            return View(HomeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
