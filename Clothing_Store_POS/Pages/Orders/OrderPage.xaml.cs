using Clothing_Store_POS.DAOs;
using Clothing_Store_POS.Models;
using Clothing_Store_POS.Services.Invoice;
using Clothing_Store_POS.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clothing_Store_POS.Pages.Orders
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OrderPage : Page
    {
        public OrdersViewModel OrdersViewModel { get; }
        
        public OrderPage()
        {
            this.InitializeComponent();
            OrdersViewModel = new OrdersViewModel();
        }

        private async void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var order = button?.CommandParameter as OrderViewModel;
            var dialog = new ContentDialog();
            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Warning";
            dialog.PrimaryButtonText = "Continue";
            dialog.CloseButtonText = "Cancel";
            dialog.Content = $"Do you really want to delete the order #{order.Id}?";

            dialog.PrimaryButtonClick += (s, args) =>
            {
                OrdersViewModel.DeleteAnOrder(order.Id);
            };

            await dialog.ShowAsync();
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

        private async void PrintBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var parameter = button?.CommandParameter;

            if (parameter is OrderViewModel orderViewModel && orderViewModel != null)
            {
                InvoiceModel invoiceModel = new InvoiceModel
                {
                    Id = orderViewModel.Id,
                    CreatedAt = orderViewModel.CreatedAt,
                    DiscountPercentage = orderViewModel.DiscountPercentage,
                    TaxPercentage = orderViewModel.TaxPercentage,
                    Note = orderViewModel.Note,
                    User = orderViewModel.User,
                    Customer = orderViewModel.Customer,
                    InvoiceItems = orderViewModel.OrderItems.Select(orderItem => new InvoiceItem
                    {
                        Product = orderItem.Product,
                        Quantity = orderItem.Quantity,
                        DiscountPercentage = orderItem.DiscountPercentage
                    }).ToList()
                };
                await InvoicePrinter.GenerateAndSaveInvoice(invoiceModel);
            }
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            OrdersViewModel.PreviousPage();
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            OrdersViewModel.NextPage();
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            OrdersViewModel.CurrentPage = 1;
            _ = OrdersViewModel.LoadOrders();
        }
    }
}
