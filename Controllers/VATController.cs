using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CoreApp.Data;
using CoreApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace CoreApp.Controllers
{
    public class VATController : Controller
    {
        private readonly ILogger<VATController> _logger;

        public VATController(ILogger<VATController> logger)
        {
            _logger = logger;
        }
        [Authorize]
        public ActionResult Index()
        {

            return RedirectToAction("EU_VAT");
        }
        [Authorize]
        public ActionResult EU_VAT(VatModel model)
        {
            if (string.IsNullOrEmpty(model.VatNumber))
                return View();
            VatModel response = new VatModel();
            var result = new VATProvider().GetVatNumberStatusAsync(model.VatNumber);
            response.VatResult = result.Result.Item1;
            response.Status = result.Result.Item2;
            response.Error = result.Result.Item3;
            return View(response);
        }

    }
}
