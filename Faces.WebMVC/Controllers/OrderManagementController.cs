using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Faces.WebMVC.RestClients;
using Microsoft.AspNetCore.Mvc;

namespace Faces.WebMVC.Controllers
{
    public class OrderManagementController : Controller
    {
        private readonly IOrderManagementApi _orderManagementApi;
        public OrderManagementController(IOrderManagementApi orderManagementApi)
        {
            _orderManagementApi = orderManagementApi;
        }
        public async Task<IActionResult> Index()
        {
            var orders = await _orderManagementApi.GetOrdersAsync();
            foreach(var order in orders)
            {
                order.ImageUrl = ConvertAndFormatToString(order.ImageData);
                order.Status = DeterMineStatus(order.Status);
            }

            return View(orders);
        }

        public async Task<IActionResult> Details(string orderId)
        {
            var order = await _orderManagementApi.GetOrderByIDAsync(Guid.Parse(orderId));
            order.ImageUrl = ConvertAndFormatToString(order.ImageData);
            order.Status = DeterMineStatus(order.Status);

            foreach(var detail in order.orderDetails)
            {
                detail.ImageUrl = ConvertAndFormatToString(detail.FaceData);
            }

            return View(order);
        }

        private string ConvertAndFormatToString(byte[] imageData)
        {
            var imageBase64Data = Convert.ToBase64String(imageData);
            return string.Format("data:image/png;base64, {0}", imageBase64Data);
        }

        private string DeterMineStatus(string status)
        {
            if (status.Equals("0"))
                return "Registered";
            else if (status.Equals("1"))
                return "Processing";
            else
                return "Sent";
        }
    }
}
