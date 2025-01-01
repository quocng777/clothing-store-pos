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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clothing_Store_POS.Pages.Categories
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CategoryPage : Page
    {
        private CategoriesViewModel _viewModel { get; }
        ObservableCollection<Category> _categories;
        public string Keyword { get; set; } = string.Empty;

        public CategoryPage()
        {
            this.InitializeComponent();
            _viewModel = new CategoriesViewModel();

            // DataContext help UserPage auto find attribute of UserViewModel
            this.DataContext = _viewModel;
        }

        private async void LoadCategories(object sender, RoutedEventArgs e)
        {
            var tempList = await _viewModel.LoadCategories();
            _categories = new ObservableCollection<Category>(tempList);

            listCategories.ItemsSource = _categories;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreateCategoryPage));
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var category = button?.CommandParameter as Category;

            Frame.Navigate(typeof(EditCategoryPage), category);
        }

        private async void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.CurrentPage = 1;
            var tempList = await _viewModel.LoadCategories();
            _categories = new ObservableCollection<Category>(tempList);

            listCategories.ItemsSource = _categories;
        }

        private async void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var category = button?.CommandParameter as Category;

            var warningDialog = new ContentDialog
            {
                Title = "Delete User",
                Content = $"Are you sure you want to delete this user ({category.Name})? This action cannot be undone.",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = this.XamlRoot
            };

            var result = await warningDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                // Delete user 
                await _viewModel.DeleteCategory(category.Id);

                var successDialog = new ContentDialog
                {
                    Title = "Delete User",
                    Content = "User has been deleted successfully.",
                    CloseButtonText = "OK"
                };

                successDialog.XamlRoot = this.XamlRoot;

                await successDialog.ShowAsync();

                LoadCategories(null, null);
            }
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.CurrentPage > 1)
            {
                _viewModel.CurrentPage--;
                LoadCategories(null, null);
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.CurrentPage < _viewModel.TotalPages)
            {
                _viewModel.CurrentPage++;
                LoadCategories(null, null);
            }
        }
    }
}
