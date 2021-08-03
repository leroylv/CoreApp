using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CoreApp.Models;
using CoreApp.Data;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;

namespace CoreApp.Controllers
{
    public class ECBController : Controller
    {
        private readonly ILogger<ECBController> _logger;
        public ECBController(ILogger<ECBController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        [Authorize]
        public virtual async Task<IActionResult> List()
        {

            var model = new CurrencyModel();

            try
            {
                var rates = await new EcbExchangeRateProvider().GetCurrencyLiveRatesAsync("eur");
                model.ExchangeRates = rates.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e,"List rates error");
            }

            return View(model);
        }

    }
}
