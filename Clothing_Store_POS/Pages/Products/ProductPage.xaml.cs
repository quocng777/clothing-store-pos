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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clothing_Store_POS.Pages.Products
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProductPage : Page
    {
        public ProductsViewModel ViewModel { get; }
        public ProductPage()
        {
            this.InitializeComponent();
            ViewModel = new ProductsViewModel();
        }

        private async void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button?.CommandParameter as Product;

            Debug.WriteLine($"Deleting product: {product.Name} with ID: {product.Id}");

            var dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Warning";
            dialog.PrimaryButtonText = "Continue";
            dialog.CloseButtonText = "Cancel";
            dialog.Content = $"Do you really want to delete {product.Name}?";

            dialog.PrimaryButtonClick += async (s, args) =>
            {
                ViewModel.DeleteAProduct(product.Id);
            };

            await dialog.ShowAsync();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button?.CommandParameter as Product;

            Frame.Navigate(typeof(EditProductPage), product);
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreateProductPage));
        }


        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            //if (ViewModel.CurrentPage > 1)
            //{
            //    ViewModel.CurrentPage--;
            //    ViewModel.LoadProducts(ViewModel.CurrentPage, 6);
            //}
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            //if (ViewModel.CurrentPage < ViewModel.TotalPages)
            //{
            //    ViewModel.CurrentPage++;
            //    ViewModel.LoadProducts(ViewModel.CurrentPage, 6);
            //}
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            //ViewModel.LoadProducts(ViewModel.CurrentPage, 6);
        }

        private void DelTextBtn_Click(object sender, RoutedEventArgs e)
        {
            //ViewModel.Keyword = "";
            //ViewModel.LoadProducts(ViewModel.CurrentPage, 6);
        }
    }
}
