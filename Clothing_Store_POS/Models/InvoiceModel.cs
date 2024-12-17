using Clothing_Store_POS.DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.Models
{
    public class InvoiceModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public float DiscountPercentage { get; set; }
        public float TaxPercentage { get; set; }
        public User User { get; set; }
        public Customer Customer { get; set; }
        public List<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
        public string Note { get; set; }

        public double OriginalPrice => InvoiceItems.Sum(ii => ii.TotalPrice);
        public double FinalPrice => OriginalPrice * (1 + TaxPercentage / 100) * (1 - DiscountPercentage / 100);

        public static InvoiceModel CreateInvoiceModelFromOrderId(int orderId)
        {
            if (orderId <= 0)
            {
                return null;
            }

            var orderDAO = new OrderDAO();
            var orderItemDAO = new OrderItemDAO();
            var productDAO = new ProductDAO();
            var userDAO = new UserDAO();
            var customerDAO = new CustomerDAO();
            Order order = orderDAO.FindOrderById(orderId);
            order.User = userDAO.FindUserById(order.UserId);
            if (order.CustomerId is int customerId && order.CustomerId > 0)
            {
                order.Customer = customerDAO.FindCustomerById(customerId);
            }
            order.OrderItems = orderItemDAO.GetOrderItemsByOrderId(order.Id);

            return new InvoiceModel
            {
                Id = order.Id,
                CreatedAt = order.CreatedAt,
                DiscountPercentage = order.DiscountPercentage,
                TaxPercentage = order.TaxPercentage,
                User = order.User,
                Customer = order.Customer,
                Note = order.Note,
                InvoiceItems = order.OrderItems.Select(orderItem => new InvoiceItem
                {
                    Product = orderItem.Product,
                    Quantity = orderItem.Quantity,
                    DiscountPercentage = orderItem.DiscountPercentage
                }).ToList()
            };
        }
    }

    public class InvoiceItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public float DiscountPercentage { get; set; }

        public double OriginalPrice => Quantity * Product.Price;
        public double TotalPrice => OriginalPrice * (1 - DiscountPercentage / 100);
    }
}
