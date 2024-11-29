using Clothing_Store_POS.Config;
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

namespace Clothing_Store_POS.Pages.Auth
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ForgotPasswordPage : Page
    {
        private EmailService _emailService { get; }

        public ForgotPasswordPage()
        {
            _emailService = new EmailService();
            this.InitializeComponent();
        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            // Validate email format 
            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, emailPattern))
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Please enter a valid email address.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                _ = errorDialog.ShowAsync();
                return;
            }

            // Show ProgressRing
            Overlay.Visibility = Visibility.Visible;

            try
            {
                await Task.Run(() =>
                {
                    // Email Context
                    var emailSubject = "Forgot Password";
                    var emailContent = "quen con cac";
                    var recipients = new List<string> { email };

                    _emailService.SendEmail(recipients, emailSubject, emailContent);
                });

                // Hide ProgressRing
                Overlay.Visibility = Visibility.Collapsed;

                // Handle email submission logic here
                ContentDialog successDialog = new ContentDialog
                {
                    Title = "Email Sent",
                    Content = "An email with password reset instructions has been sent to your email address.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await successDialog.ShowAsync();

                // Back to LoginPage
                Frame.Navigate(typeof(LoginPage));
            }
            catch (Exception ex)
            {
                Overlay.Visibility = Visibility.Collapsed;

                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = $"An error occurred: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await errorDialog.ShowAsync();
            }
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }
    }
}
