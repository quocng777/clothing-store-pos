using Clothing_Store_POS.Config;
using Clothing_Store_POS.DAOs;
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
        private FileService _fileService;
        ObservableCollection<User> _users;
        public string Keyword { get; set; } = string.Empty;

        public UserPage()
        {
            this.InitializeComponent();
            _viewModel = new UsersViewModel();
            _fileService = new FileService();

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

            var records = _fileService.ImportCsv<UserDTO>(file.Path.ToString());

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

            foreach (var user in records)
            {
                await _viewModel.AddUser(user);
            }

            var successDialog = new ContentDialog
            {
                Title = "Import CSV",
                Content = "Import successfully",
                CloseButtonText = "OK"
            };

            successDialog.XamlRoot = this.XamlRoot;

            await successDialog.ShowAsync();

            Frame.Navigate(typeof(UserPage));
        }

        private async void ExportCSV_Click(object sender, RoutedEventArgs e)
        {
            var listUsers = _users.ToList();

            // delete passwordHash
            listUsers.ForEach(u => u.PasswordHash = "_");

            _fileService.ExportCsv<User>(listUsers, "export_users.csv");

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
