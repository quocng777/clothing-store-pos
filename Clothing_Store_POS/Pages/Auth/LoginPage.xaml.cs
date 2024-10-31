using Clothing_Store_POS.Helper;
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
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clothing_Store_POS.Pages.Auth
{

    public sealed partial class LoginPage : Page
    {
        public UsersViewModel ViewModel { get; }
        public LoginPage()
        {
            this.InitializeComponent();
            ViewModel = new UsersViewModel();
        }
        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            var user = await ViewModel.GetUserByUsername(username);

            if (user == null || Utilities.VerifyPassword(password, user.PasswordHash) == false)
            {
                Frame.Navigate(typeof(LoginPage));
            }
            else
            {
                Frame.Navigate(typeof(MainLayout));
            }

        }
    }
}
