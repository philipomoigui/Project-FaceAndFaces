using Microsoft.EntityFrameworkCore;
using OrdersApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrdersApi.Persistence
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrdersContext _context;
        public OrderRepository(OrdersContext context)
        {
            _context = context;
        }
        public Order GetOrder(Guid id)
        {
            return _context.Orders
                .Include("OrderDetails")
                .FirstOrDefault(i => i.OrderId == id);
        }

        public async Task<Order> GetOrderAsync(Guid id)
        {
            return await _context.Orders
                .Include("OrderDetails")
                .FirstOrDefaultAsync(i => i.OrderId == id);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _context.Orders
                .Include("OrderDetails")
                .ToListAsync();
        }

        public Task RegisterOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();

            return Task.FromResult(true);
        }

        public void UpdateOrder(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
