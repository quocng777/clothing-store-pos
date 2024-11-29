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

namespace Clothing_Store_POS.Pages.Auth
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ResetPasswordPage : Page
    {
        public string Password { get; set; }
        public string ConfirmPassword {  get; set; }
        public string Email { get; private set; }
        private UsersViewModel _viewModel { get; }
        
        public ResetPasswordPage()
        {
            this.InitializeComponent();
            _viewModel = new UsersViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is string email)
            {
                Email = email;
            }
        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            {
                PasswordErrorText.Visibility = Visibility.Collapsed;
                ConfirmPasswordErrorText.Visibility = Visibility.Collapsed;
            }

            var passwordPattern = @"^(?=.*[0-9])(?=.*[A-Z])(?=.*[!@#\$%\^&\*])(?=.{8,})";
            if (Password == null || !Regex.IsMatch(Password, passwordPattern))
            {
                PasswordErrorText.Text = "Password must contain at least one digit, one uppercase letter, one special character, and be at least 8 characters long.";
                PasswordErrorText.Visibility = Visibility.Visible;
                return;
            }

            if (ConfirmPassword == null || ConfirmPassword != Password)
            {
                ConfirmPasswordErrorText.Text = "Passwords do not match";
                ConfirmPasswordErrorText.Visibility = Visibility.Visible;
                return;
            }

            // update password to DB
            await _viewModel.ResetPassword(Email, Password);

            var successDialog = new ContentDialog
            {
                Title = "Reset Password",
                Content = "Reset password successfully.",
                CloseButtonText = "OK"
            };

            successDialog.XamlRoot = this.XamlRoot;

            Frame.Navigate(typeof(LoginPage));
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }
    }
}
