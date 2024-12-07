using Clothing_Store_POS.Config;
using Clothing_Store_POS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public Task<PagedResult<Order>> GetListOrders(int pageNumber, int pageSize, string keyword)
        {
            var query = _context.Orders.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query
                    .Where(o => o.Id.ToString().Contains(keyword) 
                    || o.Customer.Name.Contains(keyword)
                    || o.User.UserName.Contains(keyword));

            }

            int totalItems = query.Count();

            var orders = query
                .OrderBy(o => o.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(o => o.Customer)
                .Include(o => o.User)
                .ToList();

            return Task.FromResult(new PagedResult<Order>(orders, totalItems, pageSize));
        }

        public Order FindOrderById(int orderId)
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
