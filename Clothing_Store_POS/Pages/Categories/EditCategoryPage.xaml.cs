using Clothing_Store_POS.Models;
using Clothing_Store_POS.Pages.Products;
using Clothing_Store_POS.Pages.Users;
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
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clothing_Store_POS.Pages.Categories
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditCategoryPage : Page
    {
        public CategoriesViewModel _viewModel { get; }
        public Category CurrentCategory { get; set; }
        public EditCategoryPage()
        {
            this.InitializeComponent();
            _viewModel = new CategoriesViewModel();
            CurrentCategory = new Category();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Category category)
            {
                this.CurrentCategory.Id = category.Id;
                this.CurrentCategory.Name = category.Name;
            }
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CategoryPage));
        }

        private async void ContinueBtn_Click(object sender, RoutedEventArgs e)
        {
            {
                NameErrorText.Visibility = Visibility.Collapsed;
            }

            if (CurrentCategory.Name == null || CurrentCategory.Name.Trim() == "")
            {
                NameErrorText.Text = "Name is required";
                NameErrorText.Visibility = Visibility.Visible;
                return;
            }


            var successDialog = new ContentDialog
            {
                Title = "User Saved",
                Content = "User has been saved successfully.",
                CloseButtonText = "OK"
            };

            successDialog.XamlRoot = this.XamlRoot;

            // add user to DB
            await _viewModel.UpdateCategory(CurrentCategory);

            await successDialog.ShowAsync();

            Frame.Navigate(typeof(CategoryPage));
        }

        public void CancelBtn_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Frame.Navigate(typeof(CategoryPage));
        }

        private async void CancleEditBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Warning";
            dialog.PrimaryButtonText = "OK";
            dialog.SecondaryButtonText = "Cancel";
            dialog.Content = "If your continue the processing, your temporary category's data will be clear?";

            dialog.PrimaryButtonClick += CancelBtn_Click;

            await dialog.ShowAsync();
        }
    }
}
