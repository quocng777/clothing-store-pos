using Clothing_Store_POS.Models;
using Clothing_Store_POS.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clothing_Store_POS.Pages.Products
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateProductPage : Page
    {
        public CategoriesViewModel CategoriesViewModel ;
        public ProductViewModel ProductViewModel;

        public CreateProductPage()
        {
            this.InitializeComponent();
            this.CategoriesViewModel = new CategoriesViewModel();
            this.ProductViewModel = new ProductViewModel();
        }

        private async void PickAPhotoButton_Click(object sender, RoutedEventArgs e)
        {

            // Create a file picker
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            // See the sample code below for how to make the window accessible from the App class.
            var window = (Application.Current as App).MainWindow;

            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

            // Initialize the file picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            // Set options for your file picker
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            // Open the picker for the user to pick a file
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
            if(CategoriesComboBox.SelectedItem is Category category)
            {
                if (category != null)
                {
                    this.ProductViewModel.CategoryId = category.Id;
                }
            }
        }

        private async void ContinueBtn_Click(object sender, RoutedEventArgs e)
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

            if(ProductViewModel.Name == null || ProductViewModel.Name.Trim() == "")
            {
                NameErrorText.Text = "Name is required";
                NameErrorText.Visibility = Visibility.Visible;
                return;
            }

            if (ProductViewModel.Price < 0)
            {
                PriceErrorText.Text = "Price is required";
                PriceErrorText.Visibility = Visibility.Visible;
                return;
            }

            if (ProductViewModel.Size == null || ProductViewModel.Size.Trim() == "")
            {
                SizeErrorText.Text = "Size is required";
                SizeErrorText.Visibility = Visibility.Visible;
                return;
            }

            if (ProductViewModel.Stock < 0) {
                StockErrorText.Text = "Stock value is invalid";
                StockErrorText.Visibility = Visibility.Visible;
                return;
            }

            if (ProductViewModel.Sale < 0)
            {
                SaleErrorText.Text = "Stock value is invalid";
                SaleErrorText.Visibility = Visibility.Visible;
                return;
            }

            if (ProductViewModel.CategoryId <= 0)
            {
                CategoryErrorText.Text = "Category is required";
                CategoryErrorText.Visibility = Visibility.Visible;
                return;
            }

            if (ProductViewModel.Thumbnail == null)
            {
                ThumbnailErrorText.Text = "Thumbnail image is required";
                ThumbnailErrorText.Visibility = Visibility.Visible;
                return;
            }

            var savingDialog = new ContentDialog
            {
                Title = "Saving your product",
                Content = "Please wait your product is save into our system"
            };

            savingDialog.XamlRoot = this.XamlRoot;

            savingDialog.ShowAsync();


            ProductViewModel.Save();
            savingDialog.Hide();


            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Saving product successfully.";
            dialog.PrimaryButtonText = "OK";
            dialog.SecondaryButtonText = "Cancel";
            dialog.Title = "Saving product successfully. ";
            dialog.Title = "Saving product successfully. ";
            dialog.Content = "Your product has been saved successfully. Do you want to conitnue creating a new product?";

            dialog.PrimaryButtonClick += ContinueCreatingBtn_Click;
            dialog.SecondaryButtonClick += CancelBtn_Click;

            await dialog.ShowAsync();

        }

        private void ContinueCreatingBtn_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ProductViewModel.Clear();
        }

        private void CancelBtn_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Frame.Navigate(typeof(ProductPage));
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ProductPage));
        }

        private async void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Warning";
            dialog.PrimaryButtonText = "OK";
            dialog.SecondaryButtonText = "Cancel";
            dialog.Content = "If your continue the processing, your temporary product's data will be clear?";

            dialog.PrimaryButtonClick += CancelBtn_Click;

            await dialog.ShowAsync();
        }
    }
}
