using Clothing_Store_POS.Config;
using Clothing_Store_POS.DAOs;
using Clothing_Store_POS.Models;
using Clothing_Store_POS.Pages.Categories;
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
using System.Threading.Tasks;
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
        private FileService _fileService;
        private CustomerDAO _customerDAO;

        public CustomerPage()
        {
            this.InitializeComponent();
            ViewModel = new CustomersViewModel();
            _fileService = new FileService();
            _customerDAO = new CustomerDAO();
        }

        protected override void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _ = ViewModel.LoadCustomers();
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

            Frame.Navigate(typeof(CustomerDetailPage), customer);
        }

        private async void ImportCSV_Click(object sender, RoutedEventArgs e)
        {
            // Create a file picker
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            // See the sample code below for how to make the window accessible from the App class.
            var window = (Application.Current as App).MainWindow;

            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

            // Initialize the file picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            openPicker.FileTypeFilter.Add(".csv");

            Windows.Storage.StorageFile file = await openPicker.PickSingleFileAsync();
            if (file == null)
            {
                return;
            }

            var records = _fileService.ImportCsv<Customer>(file.Path.ToString());

            if (records == null)
            {
                var errorDialog = new ContentDialog
                {
                    Title = "Import CSV",
                    Content = "Cannot import CSV file!",
                    CloseButtonText = "OK"
                };

                errorDialog.XamlRoot = this.XamlRoot;

                await errorDialog.ShowAsync();

                return;
            }

            foreach (var customer in records)
            {
                _customerDAO.AddCustomer(customer);
            }

            var successDialog = new ContentDialog
            {
                Title = "Import CSV",
                Content = "Import successfully",
                CloseButtonText = "OK"
            };

            successDialog.XamlRoot = this.XamlRoot;

            await successDialog.ShowAsync();

            Frame.Navigate(typeof(CustomerPage));
        }

        private async void ExportCSV_Click(object sender, RoutedEventArgs e)
        {
            var listCustomers = ViewModel.Customers.ToList();

            _fileService.ExportCsv<Customer>(listCustomers, "export_customers.csv");

            var successDialog = new ContentDialog
            {
                Title = "Export CSV",
                Content = "Please check csv file in Files folder",
                CloseButtonText = "OK"
            };

            successDialog.XamlRoot = this.XamlRoot;

            await successDialog.ShowAsync();
        }
    }
}
