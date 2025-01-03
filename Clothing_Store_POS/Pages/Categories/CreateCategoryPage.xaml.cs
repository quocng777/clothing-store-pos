using Clothing_Store_POS.Models;
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
    public sealed partial class CreateCategoryPage : Page
    {
        private CategoriesViewModel _viewModel { get; }
        public Category CurrentCategory { get; set; }

        public CreateCategoryPage()
        {
            this.InitializeComponent();
            CurrentCategory = new Category();
            _viewModel = new CategoriesViewModel();
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
                Title = "Category Saved",
                Content = "Category has been saved successfully.",
                CloseButtonText = "OK"
            };

            successDialog.XamlRoot = this.XamlRoot;

            // add user to DB
            await _viewModel.AddCategory(CurrentCategory);

            await successDialog.ShowAsync();

            Frame.Navigate(typeof(CategoryPage));
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CategoryPage));
        }
    }
}
