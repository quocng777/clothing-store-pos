using Clothing_Store_POS.Models;
using Clothing_Store_POS.Pages.Products;
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clothing_Store_POS.Pages.Customers
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomerPage : Page
    {
        public CustomersViewModel ViewModel { get; }

        public CustomerPage()
        {
            this.InitializeComponent();
            ViewModel = new CustomersViewModel();
        }

        private async void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var customer = button?.CommandParameter as Customer;

            Debug.WriteLine($"Deleting customer: {customer.Name} with ID: {customer.Id}");

            var dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Warning";
            dialog.PrimaryButtonText = "Continue";
            dialog.CloseButtonText = "Cancel";
            dialog.Content = $"Do you really want to delete {customer.Name}?";

            dialog.PrimaryButtonClick += async (s, args) =>
            {
                ViewModel.DeleteACustomer(customer.Id);
            };

            await dialog.ShowAsync();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var customer = button?.CommandParameter as Customer;

            Frame.Navigate(typeof(EditCustomerPage), customer);
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreateCustomerPage));
        }


        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.CurrentPage > 1)
            {
                ViewModel.CurrentPage--;
                ViewModel.LoadCustomers(ViewModel.CurrentPage, 6);
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.CurrentPage < ViewModel.TotalPages)
            {
                ViewModel.CurrentPage++;
                ViewModel.LoadCustomers(ViewModel.CurrentPage, 6);
            }
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadCustomers(ViewModel.CurrentPage, 6);
        }

        private void DelTextBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Keyword = "";
            ViewModel.LoadCustomers(ViewModel.CurrentPage, 6);
        }

        private void SendMailBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SendMailPage));
        }

        private void Customer_ItemClick(object sender, ItemClickEventArgs e)
        {

            var customer = e.ClickedItem as Customer;

            Frame.Navigate(typeof(CustomerDetailPage));
        }
    }
}
