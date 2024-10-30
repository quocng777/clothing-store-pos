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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clothing_Store_POS.Pages.Users
{
    public sealed partial class CreateUserPage : Page
    {
        private UsersViewModel _viewModel { get; }
        public UserDTO CurrentUser { get; set; }

        public CreateUserPage()
        {
            this.InitializeComponent();
            CurrentUser = new UserDTO();
        }

        private async void ContinueBtn_Click(object sender, RoutedEventArgs e)
        {
            {
                FullnameErrorText.Visibility = Visibility.Collapsed;
                UsernameErrorText.Visibility = Visibility.Collapsed;
                EmailErrorText.Visibility = Visibility.Collapsed;
                PasswordErrorText.Visibility = Visibility.Collapsed;
                ConfirmPasswordErrorText.Visibility = Visibility.Collapsed;
                RoleErrorText.Visibility = Visibility.Collapsed;
                //ThumbnailErrorText.Visibility = Visibility.Collapsed;
            }

            if (CurrentUser.FullName == null || CurrentUser.FullName.Trim() == "")
            {
                FullnameErrorText.Text = "Fullname is required";
                FullnameErrorText.Visibility = Visibility.Visible;
                return;
            }

            if (CurrentUser.UserName == null || CurrentUser.UserName.Trim() == "")
            {
                UsernameErrorText.Text = "Username is required";
                UsernameErrorText.Visibility = Visibility.Visible;
                return;
            }

            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (CurrentUser.Email == null || !Regex.IsMatch(CurrentUser.Email, emailPattern))
            {
                EmailErrorText.Text = "Invalid email format";
                EmailErrorText.Visibility = Visibility.Visible;
                return;
            }

            var passwordPattern = @"^(?=.*[0-9])(?=.*[A-Z])(?=.*[!@#\$%\^&\*])(?=.{8,})";
            if (CurrentUser.Password == null || !Regex.IsMatch(CurrentUser.Password, passwordPattern))
            {
                PasswordErrorText.Text = "Password must contain at least one digit, one uppercase letter, one special character, and be at least 8 characters long.";
                PasswordErrorText.Visibility = Visibility.Visible;
                return;
            }

            if (CurrentUser.ConfirmPassword == null || CurrentUser.ConfirmPassword != CurrentUser.Password)
            {
                ConfirmPasswordErrorText.Text = "Passwords do not match";
                ConfirmPasswordErrorText.Visibility = Visibility.Visible;
                return;
            }

            if (CurrentUser.Role == null || CurrentUser.Role.Trim() == "")
            {
                RoleErrorText.Text = "Role is required";
                RoleErrorText.Visibility = Visibility.Visible;
                return;
            }


            var successDialog = new ContentDialog
            {
                Title = "User Saved",
                Content = "User has been saved successfully.",
                CloseButtonText = "OK"
            };

            successDialog.XamlRoot = this.XamlRoot;
            await successDialog.ShowAsync();

            Frame.Navigate(typeof(UserPage));
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ContinueBtn_Click(object sender, RoutedEvent e)
        {
        
        }



    }
}
