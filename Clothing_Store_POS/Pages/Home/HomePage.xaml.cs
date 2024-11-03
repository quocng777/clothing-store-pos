using Clothing_Store_POS.DAOs;
using Clothing_Store_POS.Models;
using Clothing_Store_POS.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clothing_Store_POS.Pages.Home
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page, INotifyPropertyChanged
    {
        public ProductsViewModel ProductsViewModel { get; set; }
        public CategoriesViewModel CategoriesViewModel { get; set; }
        public ObservableCollection<CartItemViewModel> CartItems { get; set; }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }

        public double TotalAmount
        {
            get => CartItems.Sum(item => item.TotalPrice);
        }

        public HomePage()
        {
            ProductsViewModel = new ProductsViewModel();
            CategoriesViewModel = new CategoriesViewModel();
            CartItems = new ObservableCollection<CartItemViewModel>();
            CartItems.CollectionChanged += CartItems_CollectionChanged;
            this.InitializeComponent();
            this.DataContext = this;
        }

        private async void DetailButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Clicked");
            Debug.WriteLine(sender);
            if (sender is Button button && button.DataContext is Product product)
            {
                SelectedProduct = product;
                Debug.WriteLine(SelectedProduct.Name);
                await ProductDetailDialog.ShowAsync();
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button?.CommandParameter as Product;
            Debug.WriteLine($"Adding product to cart: {product.Name}");
            if (product != null) {
                var existingCartItem = CartItems.FirstOrDefault(item => item.Product.Id == product.Id);

                if (existingCartItem != null)
                {
                    Debug.WriteLine(existingCartItem.Quantity);
                    existingCartItem.IncreaseQuantity();
                }
                else
                {
                    CartItems.Add(new CartItemViewModel(product, 1));
                }
                OnPropertyChanged(nameof(CartItems));
            }
        }

        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is CartItemViewModel cartItem)
            {
                cartItem.IncreaseQuantity();
            }
        }

        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is CartItemViewModel cartItem)
            {
                cartItem.DecreaseQuantity();
            }
        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is CartItemViewModel cartItem)
            {
                CartItems.Remove(cartItem);
            }
        }

        private void CartItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (CartItemViewModel item in e.NewItems)
                {
                    item.PropertyChanged += CartItem_PropertyChanged;
                }
            }
            if (e.OldItems != null)
            {
                foreach (CartItemViewModel item in e.OldItems)
                {
                    item.PropertyChanged -= CartItem_PropertyChanged;
                }
            }

            OnPropertyChanged(nameof(TotalAmount));
        }

        private void CartItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CartItemViewModel.Quantity) || e.PropertyName == nameof(CartItemViewModel.TotalPrice))
            {
                OnPropertyChanged(nameof(TotalAmount));
            }
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Page_Submitted(object sender, RoutedEventArgs e)
        {

        }

        private void SaveOrder_Click(object sender, RoutedEventArgs e)
        {
            var newOrder = new OrderViewModel
            {
                CreatedAt = DateTime.UtcNow,
                OrderItems = CartItems.Select(cartItem => new OrderItemViewModel
                {
                    ProductId = cartItem.Product.Id,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Product.Price,
                }).ToList()
            };

        }

        private void SaveAndPrintOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CategoryItem_Click(object sender, RoutedEventArgs e)
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
