using Clothing_Store_POS.Config;
using Clothing_Store_POS.DAOs;
using Clothing_Store_POS.Models;
using Clothing_Store_POS.Pages.Users;
using Clothing_Store_POS.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using System.Linq;
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
        private ProductViewModel _productViewModel;
        private ProductDAO _productDAO;
        private FileService _fileService;
        public ProductPage()
        {
            this.InitializeComponent();
            ViewModel = new ProductsViewModel();
            _fileService = new FileService();
            _productDAO = new ProductDAO();
            _productViewModel = new ProductViewModel();
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
            ViewModel.PreviousPage();
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NextPage();
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CurrentPage = 1;
            _ = ViewModel.LoadProducts();
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

            var records = _fileService.ImportCsv<Product>(file.Path.ToString());

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

            foreach (var product in records)
            {
                product.Thumbnail = _productViewModel.SaveThumbnailImage(product.Thumbnail);
                _productDAO.AddProduct(product);
            }

            var successDialog = new ContentDialog
            {
                Title = "Import CSV",
                Content = "Import successfully",
                CloseButtonText = "OK"
            };

            successDialog.XamlRoot = this.XamlRoot;

            await successDialog.ShowAsync();

            Frame.Navigate(typeof(ProductPage));
        }

        private async void ExportCSV_Click(object sender, RoutedEventArgs e)
        {
            var listProducts = ViewModel.Products.ToList();

            _fileService.ExportCsv<Product>(listProducts, "export_products.csv");

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
