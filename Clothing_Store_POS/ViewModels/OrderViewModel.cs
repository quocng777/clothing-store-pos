using Clothing_Store_POS.DAOs;
using Clothing_Store_POS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public User User { get; set; }
        public Customer Customer { get; set; }
        public ObservableCollection<OrderItemViewModel> OrderItems { get; set; }
        public string Note { get; set; }
        public double OriginalPrice => OrderItems.Sum(oi => oi.TotalPrice);
        public double FinalPrice => OriginalPrice * (1 + TaxPercentage / 100) * (1 - DiscountPercentage / 100);

        public OrderViewModel()
        {
            _orderDAO = new OrderDAO();
            _orderItemDAO = new OrderItemDAO();
            DiscountPercentage = 0;
            TaxPercentage = 0;
        }

        public OrderViewModel(Order order)
        {
            _orderDAO = new OrderDAO();
            _orderItemDAO = new OrderItemDAO();
            Id = order.Id;
            CreatedAt = order.CreatedAt.ToLocalTime();
            DiscountPercentage = order.DiscountPercentage;
            TaxPercentage = order.TaxPercentage;
            User = order.User;
            Customer = order.Customer;
            Note = order.Note;
        }

        public void LoadOrderItems()
        {
            var orderItems = _orderItemDAO.GetOrderItemsByOrderId(Id);
            OrderItems = new ObservableCollection<OrderItemViewModel>();
            foreach (var orderItem in orderItems)
            {
                OrderItems.Add(new OrderItemViewModel(orderItem));
            }
        }

        public int CreateOrder(int customerId, int userId)
        {
            var order = new Order
            {
                DiscountPercentage = DiscountPercentage,
                TaxPercentage = TaxPercentage,
                CustomerId = customerId != -1 ? customerId : null,
                UserId = userId,
                Note = Note,
                CreatedAt = DateTime.UtcNow,
            };
            int orderId = _orderDAO.AddOrder(order);
            return orderId;
        }

        public void DeleteOrder()
        {
            _orderDAO.DeleteOrderById(Id);
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
