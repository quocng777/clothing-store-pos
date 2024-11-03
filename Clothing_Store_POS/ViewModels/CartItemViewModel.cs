using Clothing_Store_POS.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Clothing_Store_POS.ViewModels
{
    public class CartItemViewModel : INotifyPropertyChanged
    {
        public Product Product { get; set; }

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
        public double TotalPrice => this.Product.Price * this.Quantity;

        public CartItemViewModel(Product product, int quantity)
        {
            this.Product = product;
            this.Quantity = quantity;
        }

        public void IncreaseQuantity()
        {
            this.Quantity++;
        }

        public void DecreaseQuantity()
        {
            if (this.Quantity > 1)
            {
                this.Quantity--;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
