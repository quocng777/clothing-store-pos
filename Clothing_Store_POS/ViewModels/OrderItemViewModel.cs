using Clothing_Store_POS.DAOs;
using Clothing_Store_POS.Models;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.ViewModels
{
    public class OrderItemViewModel
    {
        private readonly OrderItemDAO _orderItemDAO;
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public OrderItemViewModel()
        {
            _orderItemDAO = new OrderItemDAO();
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
