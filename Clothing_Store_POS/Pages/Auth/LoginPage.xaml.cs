using Clothing_Store_POS.Config;
using Clothing_Store_POS.Helper;
using Clothing_Store_POS.Models;
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

            // check localSettings for Remember Me
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            if (localSettings.Values.ContainsKey("rememberMe"))
            {
                UsernameTextBox.Text = localSettings.Values["username"] as string;
                PasswordBox.Password = localSettings.Values["password"] as string;
                RememberMeCheckBox.IsChecked = (bool)localSettings.Values["rememberMe"];
            }
        }
        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            {
                UsernameErrorText.Visibility = Visibility.Collapsed;    
                PasswordErrorText.Visibility = Visibility.Collapsed;
                LoginErrorText.Visibility = Visibility.Collapsed;
            }

            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            // validate input
            if (username == null || username.Trim() == "")
            {
                UsernameErrorText.Text = "Username is required";
                UsernameErrorText.Visibility = Visibility.Visible;
                return;
            }

            if (password == null || password.Trim() == "")
            {
                PasswordErrorText.Text = "Password is required";
                PasswordErrorText.Visibility = Visibility.Visible;
                return;
            }

            // Show ProgressRing
            Overlay.Visibility = Visibility.Visible;

            // if `Remember Me` save username & password to localSettings
            if (RememberMeCheckBox.IsChecked == true)
            {
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["username"] = username;
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["password"] = password;
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["rememberMe"] = true;
            }
            else
            {
                // If none, delete in localSettings
                Windows.Storage.ApplicationData.Current.LocalSettings.Values.Remove("username");
                Windows.Storage.ApplicationData.Current.LocalSettings.Values.Remove("password");
                Windows.Storage.ApplicationData.Current.LocalSettings.Values.Remove("rememberMe");
            }

            var user = await ViewModel.GetUserByUsername(username);

            if (user == null || Utilities.VerifyPassword(password, user.PasswordHash) == false)
            {
                LoginErrorText.Visibility = Visibility.Visible;
                UsernameTextBox.Text = string.Empty;
                PasswordBox.Password = string.Empty;
                return;
            }
            else
            {
                AppSession.CurrentUser = user;

                // Hide ProgressRing
                Overlay.Visibility = Visibility.Collapsed;

                Frame.Navigate(typeof(MainLayout));
            }

        }

        private void ForgotPass_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ForgotPasswordPage));
        }
    }
}
