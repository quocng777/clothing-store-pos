using Clothing_Store_POS.Converters;
using Clothing_Store_POS.Models;
using Clothing_Store_POS.ViewModels;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Diagnostics;
using Windows.Storage;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clothing_Store_POS.Pages.Products
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditProductPage : Page
    {
        public ProductViewModel ProductViewModel { get; set; }
        public CategoriesViewModel CategoriesViewModel { get; set; }
        private int _fromPage = 1;

        public EditProductPage()
        {
            this.InitializeComponent();
            ProductViewModel = new ProductViewModel();
        }

        protected override async void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.CategoriesViewModel = new CategoriesViewModel();
            await CategoriesViewModel.InitializeAsync();

            if (e.Parameter != null)
            {
                try
                {
                    dynamic parameters = e.Parameter;

                    var product = parameters.Product as ProductViewModel;
                    var currentPage = parameters.Page as int?;

                    if (product != null)
                    {
                        Debug.WriteLine($"Navigated to EditProductPage with product: {product.Name}");
                        this.ProductViewModel = product;

                        CategoriesComboBox.SelectedValue = ProductViewModel.CategoryId;
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

        private void CategoriesComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (CategoriesComboBox.ItemsSource != null && ProductViewModel.CategoryId != 0)
            {
                CategoriesComboBox.SelectedValue = ProductViewModel.CategoryId;
            }
        }

        private async void PickAPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            var window = (Application.Current as App).MainWindow;
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                ProductViewModel.Thumbnail = file.Path.ToString();
                BitmapImage bitmap = new BitmapImage();
                using (var stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    await bitmap.SetSourceAsync(stream);
                }

                SelectedImage.Source = bitmap;
            }
            else
            {

            }
        }

        private void CategoriesBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoriesComboBox.SelectedItem is int categoryId)
            {
                if (categoryId > 0)
                {
                    this.ProductViewModel.CategoryId = categoryId;
                }
            }
        }

        private void PriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = PriceTextBox.Text;
            string cleanedText = System.Text.RegularExpressions.Regex.Replace(text, @"[^0-9]", "");

            if (double.TryParse(cleanedText, out double price))
            {
                ProductViewModel.Price = price;

                PriceTextBox.Text = PriceToVNDConverter.ConvertToVND(price);

                PriceTextBox.SelectionStart = PriceTextBox.Text.Length - 2;
            }
        }

        private bool ValidateForm()
        {
            {
                NameErrorText.Visibility = Visibility.Collapsed;
                PriceErrorText.Visibility = Visibility.Collapsed;
                SizeErrorText.Visibility = Visibility.Collapsed;
                StockErrorText.Visibility = Visibility.Collapsed;
                SaleErrorText.Visibility = Visibility.Collapsed;
                CategoryErrorText.Visibility = Visibility.Collapsed;
                ThumbnailErrorText.Visibility = Visibility.Collapsed;
            }

            if (string.IsNullOrWhiteSpace(ProductViewModel.Name))
            {
                NameErrorText.Text = "Name is required";
                NameErrorText.Visibility = Visibility.Visible;
                return false;
            }

            if (ProductViewModel.Price <= 0)
            {
                PriceErrorText.Text = "Price is required";
                PriceErrorText.Visibility = Visibility.Visible;
                return false;
            }

            if (string.IsNullOrWhiteSpace(ProductViewModel.Size))
            {
                SizeErrorText.Text = "Size is required";
                SizeErrorText.Visibility = Visibility.Visible;
                return false;
            }

            if (string.IsNullOrEmpty(StockTextBox.Text) || (!int.TryParse(StockTextBox.Text, out int stock) || stock <= 0))
            {
                StockErrorText.Text = "Stock must be a non-negative integer";
                StockErrorText.Visibility = Visibility.Visible;
                return false;
            }
            else
            {
                ProductViewModel.Stock = stock;
            }

            if (string.IsNullOrEmpty(SaleTextBox.Text) || (!float.TryParse(SaleTextBox.Text, out float sale) || sale < 0 || sale > 100))
            {
                SaleErrorText.Text = "Sale must be between 0 and 100";
                SaleErrorText.Visibility = Visibility.Visible;
                return false;
            }
            else
            {
                ProductViewModel.Sale = sale;
            }

            if (ProductViewModel.CategoryId <= 0)
            {
                CategoryErrorText.Text = "Category is required";
                CategoryErrorText.Visibility = Visibility.Visible;
                return false;
            }

            if (ProductViewModel.Thumbnail == null)
            {
                ThumbnailErrorText.Text = "Thumbnail image is required";
                ThumbnailErrorText.Visibility = Visibility.Visible;
                return false;
            }

            return true;
        }

        private async void ContinueBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
            {
                return;
            }

            var savingDialog = new ContentDialog
            {
                Title = "Saving your product",
                Content = "Please wait your product is save into our system"
            };
            savingDialog.XamlRoot = this.XamlRoot;
            _ = savingDialog.ShowAsync();

            ProductViewModel.Update();
            savingDialog.Hide();

            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Saving product successfully.";
            dialog.PrimaryButtonText = "OK";
            dialog.SecondaryButtonText = "Cancel";
            dialog.Content = "Your product has been saved successfully. Do you want to conitnue update this product?";
            dialog.SecondaryButtonClick += CancelDialogBtn_Click;

            await dialog.ShowAsync();
        }

        public void CancelDialogBtn_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Frame.Navigate(typeof(ProductPage), _fromPage);
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ProductPage), _fromPage);
        }

        private async void CancelEditBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Warning";
            dialog.PrimaryButtonText = "OK";
            dialog.SecondaryButtonText = "Cancel";
            dialog.Content = "If your continue the processing, your temporary product's data will be clear?";

            dialog.PrimaryButtonClick += CancelDialogBtn_Click;

            await dialog.ShowAsync();
        }   
    }
}
