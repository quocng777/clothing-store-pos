using Clothing_Store_POS.Models;
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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clothing_Store_POS.Pages.Users
{
    public sealed partial class UserPage : Page
    {
        private UsersViewModel _viewModel { get; }
        ObservableCollection<User> _users;
        public string Keyword { get; set; } = string.Empty;

        public UserPage()
        {
            this.InitializeComponent();
            _viewModel = new UsersViewModel();

            // DataContext help UserPage auto find attribute of UserViewModel
            this.DataContext = _viewModel;
        }

        private async void LoadUsers(object sender, RoutedEventArgs e)
        {
            var tempList = await _viewModel.LoadUsers();
            _users = new ObservableCollection<User>(tempList);

            listUsers.ItemsSource = _users;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreateUserPage));
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var user = button?.CommandParameter as User;

            Frame.Navigate(typeof(EditUserPage), user);
        }

        private async void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.CurrentPage = 1;
            var tempList = await _viewModel.LoadUsers();
            _users = new ObservableCollection<User>(tempList);

            listUsers.ItemsSource = _users;
        }

        private async void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var user = button?.CommandParameter as User;

            var warningDialog = new ContentDialog
            {
                Title = "Delete User",
                Content = $"Are you sure you want to delete this user ({user.UserName})? This action cannot be undone.",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = this.XamlRoot 
            };

            var result = await warningDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                // Delete user 
                await _viewModel.DeleteUser(user.Id);

                var successDialog = new ContentDialog
                {
                    Title = "Delete User",
                    Content = "User has been deleted successfully.",
                    CloseButtonText = "OK"
                };

                successDialog.XamlRoot = this.XamlRoot;

                await successDialog.ShowAsync();

                LoadUsers(null, null);
            }
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.CurrentPage > 1)
            {
                _viewModel.CurrentPage--;
                LoadUsers(null, null);
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.CurrentPage < _viewModel.TotalPages)
            {
                _viewModel.CurrentPage++;
                LoadUsers(null, null);
            }
        }
    }
}
