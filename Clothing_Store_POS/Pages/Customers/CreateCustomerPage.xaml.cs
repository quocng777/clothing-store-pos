using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Clothing_Store_POS.Models;
using Clothing_Store_POS.ViewModels;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage.Pickers;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clothing_Store_POS.Pages.Customers
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateCustomerPage : Page
    {
        public CustomerViewModel ViewModel;

        public CreateCustomerPage()
        {
            this.InitializeComponent();
            this.ViewModel = new CustomerViewModel();
        }

        private async void ContinueBtn_Click(object sender, RoutedEventArgs e)
        {
            {
                NameErrorText.Visibility = Visibility.Collapsed;
            }

            if (ViewModel.Name == null || ViewModel.Name.Trim() == "")
            {
                NameErrorText.Text = "Name is required";
                NameErrorText.Visibility = Visibility.Visible;
                return;
            }


            var savingDialog = new ContentDialog
            {
                Title = "Saving customer",
                Content = "Please waiting, your customer is saveing into our system"
            };

            savingDialog.XamlRoot = this.XamlRoot;

            savingDialog.ShowAsync();


            ViewModel.Save();
            savingDialog.Hide();


            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.PrimaryButtonText = "OK";
            dialog.SecondaryButtonText = "Cancel";
            dialog.Title = "Saving customer successfully. ";
            dialog.Content = "Your product has been saved successfully. Do you want to conitnue creating a new product?";

            dialog.PrimaryButtonClick += ContinueCreatingBtn_Click;
            dialog.SecondaryButtonClick += CancelBtn_Click;

            await dialog.ShowAsync();

        }

        private void ContinueCreatingBtn_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ViewModel.Clear();
        }

        private void CancelBtn_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Frame.Navigate(typeof(CustomerPage));
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CustomerPage));

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
            dialog.Content = "If your continue the processing, your temporary customer's data will be clear?";

            dialog.PrimaryButtonClick += CancelBtn_Click;

            await dialog.ShowAsync();
        }
    }
}
