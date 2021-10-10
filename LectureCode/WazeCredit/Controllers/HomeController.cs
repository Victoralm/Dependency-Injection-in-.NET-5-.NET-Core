using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WazeCredit.Data;
using WazeCredit.Models;
using WazeCredit.Models.ViewModels;
using WazeCredit.Services;
using WazeCredit.Utility.AppSettingsClasses;

namespace WazeCredit.Controllers
{
    public class HomeController : Controller
    {
        public HomeVM HomeVM { get; set; }
        private readonly IMarketForecaster _marketForecaster;
        private readonly ICreditValidator _creditValidator;
        private readonly ApplicationDbContext _db;

        // Already comes registered on the application stack by default, doesn't need to be registered manually on Startup
        private readonly ILogger _logger;

        [BindProperty]
        public CreditApplication CreditModel { get; set; }

        /// <summary>
        /// Injecting IMarketForecaster as a Dependency
        /// Injecting IOptions<StripeSettings>, IOptions<WazeForecastSettings>, IOptions<TwilioSettings>, IOptions<SendGridSettings> as a Dependency
        /// </summary>
        public HomeController(IMarketForecaster marketForecaster, ICreditValidator creditValidator, ApplicationDbContext db, ILogger<HomeController> logger)
        {
            this._marketForecaster = marketForecaster;
            this._creditValidator = creditValidator;
            this._db = db;

            this._logger = logger;

            HomeVM = new HomeVM();
        }

        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            this._logger.LogInformation("Home Controller Index Action Called");

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

            this._logger.LogInformation("Home Controller Index Action Ended");

            return View(HomeVM);
        }

        public IActionResult AllConfigSettings(
            [FromServices] IOptions<StripeSettings> stripeOptions,
            [FromServices] IOptions<WazeForecastSettings> wazeForecastOptions,
            [FromServices] IOptions<TwilioSettings> twilioOptions,
            [FromServices] IOptions<SendGridSettings> sendGridOptions
            )
        {
            List<string> messages = new List<string>();
            messages.Add($"Waze config - Forecast Tracker: {wazeForecastOptions.Value.ForecastTrackerEnabled}");
            messages.Add($"Stripe config - Secret Key: {stripeOptions.Value.SecretKey}");
            messages.Add($"Stripe config - Publishable Key: {stripeOptions.Value.PublishableKey}");
            messages.Add($"Twilio config - Phone Number: {twilioOptions.Value.PhoneNumber}");
            messages.Add($"Twilio config - AuthToken: {twilioOptions.Value.AuthToken}");
            messages.Add($"Twilio config - Account Sid: {twilioOptions.Value.AccountSid}");
            messages.Add($"SendGrid config - Send GridKey: {sendGridOptions.Value.SendGridKey}");

            return View(messages);
        }

        public IActionResult CreditApplication()
        {
            CreditModel = new CreditApplication();

            return View(CreditModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName("CreditApplication")]
        public async Task<IActionResult> CreditApplicationPost(
            [FromServices] Func<CreditApprovedEnum, ICreditApproved> creditService
            )
        {
            if (ModelState.IsValid)
            {
                var (validationPassed, errorMessages) = await this._creditValidator.PassAllValidations(CreditModel);

                CreditResult creditResult = new CreditResult()
                {
                    ErrorList = errorMessages,
                    CreditID = 0,
                    Success = validationPassed,
                };

                if (validationPassed)
                {
                    CreditModel.CreditApproved = creditService(CreditModel.Salary > 50000 ? CreditApprovedEnum.High : CreditApprovedEnum.Low)
                                                    .GetCreditApproved(CreditModel);
                    // add record to database
                    this._db.CreditApplicationModel.Add(CreditModel);
                    this._db.SaveChanges();
                    creditResult.CreditID = CreditModel.Id;
                    creditResult.CreditApproved = CreditModel.CreditApproved;
                    return RedirectToAction(nameof(CreditResult), creditResult);
                }
                else
                {
                    // don't add record to database, but return the values to the view
                    return RedirectToAction(nameof(CreditResult), creditResult);
                }
            }
            return View(CreditModel);
        }

        public IActionResult CreditResult(CreditResult creditResult)
        {
            return View(creditResult);
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
