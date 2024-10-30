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

        public UserPage()
        {
            this.InitializeComponent();
            _viewModel = new UsersViewModel();
        }

        private void LoadUsers(object sender, RoutedEventArgs e)
        {
            var tempList = _viewModel.GetAllUsers();
            _users = new ObservableCollection<User>(tempList);

            listUsers.ItemsSource = _users;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreateUserPage));
        }
    }
}
