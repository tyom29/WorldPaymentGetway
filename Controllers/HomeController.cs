using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Worldpay.Sdk;
using Worldpay.Sdk.Enums;
using Worldpay.Sdk.Models;
using WorldPaymentGetway.Models;

namespace WorldPaymentGetway.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WorldPaySettings _settings;

        public HomeController(ILogger<HomeController> logger, IOptions<WorldPaySettings> _options)
        {
            _settings = _options.Value; // injection
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Complete(PayModelView pay)
        {
            WorldpayRestClient restClient = new WorldpayRestClient("https://api.worldpay.com/v1", _settings.ServiceKey);

            var orderRequest = new OrderRequest()
            {
                token = pay.Token,
                amount = Convert.ToInt32(pay.Total),
                currencyCode = CurrencyCode.GBP.ToString(),
                name = "test name",
                orderDescription = "Order description",
                customerOrderCode = "Order code"
            };

            var address = new Address()
            {
                address1 = "123 House Road",
                address2 = "A village",
                city = "London",
                countryCode = CountryCode.GB.ToString(),
                postalCode = "EC1 1AA"
            };

            orderRequest.billingAddress = address;

            try
            {
                OrderResponse orderResponse = restClient.GetOrderService().Create(orderRequest);
                Console.WriteLine("Order code: " + orderResponse.orderCode);
            }
            catch (WorldpayException e)
            {
                Console.WriteLine("Error code:" + e.apiError.customCode);
                Console.WriteLine("Error description: " + e.apiError.description);
                Console.WriteLine("Error message: " + e.apiError.message);
            }

            return View();
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
