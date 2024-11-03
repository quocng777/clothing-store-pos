using Clothing_Store_POS.Config;
using Clothing_Store_POS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.DAOs
{
    public class OrderDAO
    {
        private readonly AppDBContext _context;

        public OrderDAO()
        {
            _context = new AppDBContext();
        }

        public int AddOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order.Id;
        }

        public List<Order> GetOrders()
        {
            return _context.Orders.ToList();
        }

        public Order GetOrderById(int orderId)
        {
            return _context.Orders.Find(orderId);
        }

        public void DeleteOrderById(int orderId)
        {
            var order = _context.Orders.Find(orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }
    }
}
