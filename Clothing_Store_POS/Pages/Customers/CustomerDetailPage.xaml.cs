using Clothing_Store_POS.DAOs;
using Clothing_Store_POS.Models;
using Clothing_Store_POS.Services.Invoice;
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
    public sealed partial class CustomerDetailPage : Page
    {
        public CustomerViewModel ViewModel;
        public OrdersViewModel OrdersViewModel { get; set; }
        public OrderViewModel OrderViewModel { get; set; }

        public CustomerDetailPage()
        {
            this.InitializeComponent();
            ViewModel = new CustomerViewModel();
        }

        protected override void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Customer customer)
            {
                this.ViewModel.Id = customer.Id;
                this.ViewModel.Name = customer.Name;
                this.ViewModel.Email = customer.Email;
                this.ViewModel.Phone = customer.Phone;

                OrdersViewModel = new OrdersViewModel(customer.Id);
            }
        }

        private async void ViewBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var order = button?.CommandParameter as Order;

            if (order != null)
            {
                OrderViewModel = new OrderViewModel(order);
                OrderViewModel.LoadOrderItems();
                OrderDetailsDialog.DataContext = OrderViewModel;
                await OrderDetailsDialog.ShowAsync();
            }
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CustomerPage));

        }

        private async void PrintBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var order = button?.CommandParameter as Order;

            if (order != null)
            {
                order.OrderItems = new OrderItemDAO().GetOrderItemsByOrderId(order.Id);
                InvoiceModel invoiceModel = new InvoiceModel
                {
                    Id = order.Id,
                    CreatedAt = order.CreatedAt,
                    DiscountPercentage = order.DiscountPercentage,
                    TaxPercentage = order.TaxPercentage,
                    User = order.User,
                    Customer = order.Customer,
                    InvoiceItems = order.OrderItems.Select(orderItem => new InvoiceItem
                    {
                        Product = orderItem.Product,
                        Quantity = orderItem.Quantity,
                        DiscountPercentage = orderItem.DiscountPercentage
                    }).ToList()
                };

                await InvoicePrinter.GenerateAndSaveInvoice(invoiceModel);
            }
        }
    }
}
