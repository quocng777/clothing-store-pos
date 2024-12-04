using Clothing_Store_POS.Config;
using Clothing_Store_POS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.DAOs
{
    public class OrderItemDAO
    {
        private readonly AppDBContext _context;

        public OrderItemDAO()
        {
            _context = new AppDBContext();
        }

        public int AddOrderItem(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
            _context.SaveChanges();
            return orderItem.Id;
        }

        public List<OrderItem> GetOrderItemsByOrderId(int orderId)
        {
            return _context.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .Include(oi => oi.Product)
                .ToList();
        }

        public void DeleteOrderItemById(int orderItemId)
        {
            var orderItem = _context.OrderItems.Find(orderItemId);
            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
                _context.SaveChanges();
            }
        }

        public void UpdateOrderItem(OrderItem orderItem)
        {
            _context.OrderItems.Update(orderItem);
            _context.SaveChanges();
        }
    }
}
