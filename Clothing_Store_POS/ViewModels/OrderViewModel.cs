using Clothing_Store_POS.DAOs;
using Clothing_Store_POS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.ViewModels
{
    public class OrderViewModel : INotifyPropertyChanged
    {
        private readonly OrderDAO _orderDAO;
        private readonly OrderItemDAO _orderItemDAO;
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public float DiscountPercentage { get; set; }
        public float TaxPercentage { get; set; }

        public OrderViewModel()
        {
            _orderDAO = new OrderDAO();
            _orderItemDAO = new OrderItemDAO();
            DiscountPercentage = 0;
            TaxPercentage = 0;
        }

        public int CreateOrder()
        {
            var order = new Order
            {
                DiscountPercentage = DiscountPercentage,
                TaxPercentage = TaxPercentage,
                CreatedAt = DateTime.UtcNow
            };
            int orderId = _orderDAO.AddOrder(order);
            return orderId;
        }

        public void AddOrderItem(OrderItem orderItem)
        {
            _orderItemDAO.AddOrderItem(orderItem);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
