using Clothing_Store_POS.Models;
using Clothing_Store_POS.Services.Invoice;
using Clothing_Store_POS.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using System.Linq;

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
        private int _fromPage = 1;

        public CustomerDetailPage()
        {
            this.InitializeComponent();
            ViewModel = new CustomerViewModel();
        }

        protected override void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            //base.OnNavigatedTo(e);

            //if (e.Parameter is Customer customer)
            //{
            //    this.ViewModel.Id = customer.Id;
            //    this.ViewModel.Name = customer.Name;
            //    this.ViewModel.Email = customer.Email;
            //    this.ViewModel.Phone = customer.Phone;

            //    OrdersViewModel = new OrdersViewModel(customer.Id);
            //}
            base.OnNavigatedTo(e);

            if (e.Parameter != null)
            {
                try
                {
                    dynamic parameters = e.Parameter;
                    var customer = parameters.Customer as Customer;
                    var currentPage = parameters.Page as int?;

                    if (customer != null)
                    {
                        ViewModel.Id = customer.Id;
                        ViewModel.Name = customer.Name;
                        ViewModel.Email = customer.Email;
                        ViewModel.Phone = customer.Phone;

                        OrdersViewModel = new OrdersViewModel(customer.Id);
                    }

                    if (currentPage != null)
                    {
                        _fromPage = currentPage.Value;
                    }
                }
                catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException ex)
                {
                    Debug.WriteLine($"Error accessing dynamic properties: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Unexpected error: {ex.Message}");
                }
            }
        }

        private async void ViewBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var order = button?.CommandParameter as OrderViewModel;

            if (order != null)
            {
                if (order.OrderItems == null)
                {
                    order.LoadOrderItems();
                }
                OrderDetailsDialog.DataContext = order;
                await OrderDetailsDialog.ShowAsync();
            }
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CustomerPage), _fromPage);
        }

        private async void PrintBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var order = button?.CommandParameter as OrderViewModel;

            if (order != null)
            {
                if (order.OrderItems == null)
                {
                    order.LoadOrderItems();
                }
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
