using Clothing_Store_POS.Config;
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
    public sealed partial class VerifyResetPasswordTokenPage : Page
    {
        private UsersViewModel _viewModel { get; }

        public VerifyResetPasswordTokenPage()
        {
            this.InitializeComponent();
            _viewModel = new UsersViewModel();
        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            var token = TokenTextBox.Text;

            // Validate token 
            if (string.IsNullOrWhiteSpace(token))
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Please enter token",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                _ = errorDialog.ShowAsync();
                return;
            }

            var isValidToken = AppSession.IsTokenValid(token, out var email);

            if (!isValidToken)
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Invalid Token",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                _ = errorDialog.ShowAsync();
                return;
            }

            Frame.Navigate(typeof(ResetPasswordPage), email);
        }
    }
}
