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
    public class OrderItemViewModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public float DiscountPercentage { get; set; }
        public double TotalPrice
        {
            get => Quantity * Product.Price * (1 - DiscountPercentage / 100);
        }

        public OrderItemViewModel(OrderItem orderItem)
        {
            Id = orderItem.Id;
            OrderId = orderItem.OrderId;
            Product = orderItem.Product;
            Quantity = orderItem.Quantity;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
