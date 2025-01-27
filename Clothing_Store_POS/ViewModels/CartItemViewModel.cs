﻿using Clothing_Store_POS.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Clothing_Store_POS.ViewModels
{
    public class CartItemViewModel : INotifyPropertyChanged
    {
        public ProductViewModel Product { get; set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                    OnPropertyChanged(nameof(TotalPrice));
                }
            }
        }
        public double OriginalPrice => this.Product.Price * this.Quantity;

        // Discount options
        private float _discountPercentage;
        public float DiscountPercentage
        {
            get => _discountPercentage;
            set
            {
                if (_discountPercentage != value)
                {
                    _discountPercentage = value;
                    OnPropertyChanged(nameof(TotalPrice));
                }
            }
        }

        private double _discountFixed;
        public double DiscountFixed
        {
            get => _discountFixed;
            set
            {
                if (_discountFixed != value)
                {
                    _discountFixed = value;
                    OnPropertyChanged(nameof(TotalPrice));
                }
            }
        }

        public double TotalPrice
        {
            get
            {
                double discountAmount = DiscountFixed > 0 ? DiscountFixed : OriginalPrice * (DiscountPercentage / 100);
                return Math.Max(OriginalPrice - discountAmount, 0);
            }
        }

        public string Note { get; set; }


        public CartItemViewModel(ProductViewModel product, int quantity)
        {
            this.Product = product;
            this.Quantity = quantity;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
