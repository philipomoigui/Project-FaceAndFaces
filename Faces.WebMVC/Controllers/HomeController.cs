using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Faces.WebMVC.Models;
using Faces.WebMVC.ViewModel;
using System.IO;
using Messaging.InterfacesConstant.Constants;
using MassTransit;
using Messaging.InterfacesConstant.Commands;

namespace Faces.WebMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBusControl _busControl;

        public HomeController(ILogger<HomeController> logger, IBusControl busControl)
        {
            _logger = logger;
            _busControl = busControl;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterOrder()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterOrder(OrderViewModel model)
        {
            MemoryStream ms = new MemoryStream();
            using(var uploadedFile = model.File.OpenReadStream())
            {
                await uploadedFile.CopyToAsync(ms);
            }

            model.ImageData = ms.ToArray();
            model.ImageUrl = model.File.FileName;
            model.OrderId = Guid.NewGuid();

            var sendUrl = new Uri($"{RabbitMqMassTransitConstants.RabbitMqUri}" + $"{RabbitMqMassTransitConstants.RegisterOrderCommandQueue}");
            var endpoint = await _busControl.GetSendEndpoint(sendUrl);

            await endpoint.Send<IRegisterOrderCommand>(
                new
                {
                    model.OrderId,
                    model.ImageUrl,
                    model.UserEmail,
                    model.ImageData
                }
             );

            ViewBag.OrderId = model.OrderId;

            return View("Thanks");

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
