using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrdersApi.Persistence;

namespace OrdersApi.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _repository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _repository = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var data = await _repository.GetOrdersAsync();
            return Ok(data);
        }

        [HttpGet]
        [Route("{orderId}")]
        public async Task<IActionResult> GetOrderById(string orderId)
        {
            var order = await _repository.GetOrderAsync(Guid.Parse(orderId));
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
    }
}
