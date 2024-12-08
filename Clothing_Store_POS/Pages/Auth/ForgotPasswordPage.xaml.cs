using Clothing_Store_POS.Config;
using Clothing_Store_POS.Helper;
using Clothing_Store_POS.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
        private UsersViewModel _viewModel { get; }

        public ForgotPasswordPage()
        {
            _emailService = new EmailService();
            _viewModel = new UsersViewModel();
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

            // Validate email
            var validEmail = await _viewModel.GetUserByEmail(email);

            if (validEmail == null)
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Email isn't existed.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };

                // Hide ProgressRing
                Overlay.Visibility = Visibility.Collapsed;

                _ = errorDialog.ShowAsync();
                return;
            }

            try
            {
                var token = Utilities.GenerateToken();

                // Send email
                await Task.Run(() =>
                {

                    // Email Context
                    var emailSubject = "Forgot Password";
                    var body = @"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Email Verification</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f0f0f0;
            padding: 20px;
        }
        .verify-container {
            max-width: 600px;
            margin: 20px auto;
            background: #fff;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }
        .verify-container h2 {
            text-align: center;
            margin-bottom: 30px;
        }
        .verify-content {
            font-size: 16px;
            line-height: 1.6;
            margin-bottom: 20px;
        }
        .verify-button {
            display: inline-block;
            background-color: #4CAF50;
            color: white !important;
            text-decoration: none;
            padding: 10px 20px;
            border-radius: 4px;
            text-align: center;
            font-size: 16px;
            transition: background-color 0.3s ease;
        }
        .verify-button:hover {
            background-color: #45a049;
        }
        .footer {
            margin-top: 20px;
            font-size: 14px;
            color: #666;
            text-align: center;
        }
    </style>
</head>
<body>
    <div class=""verify-container"">
        <h2>Email Verification</h2>
        <div class=""verify-content"">
            <p>Dear User,</p>
            <p>Here is your private token:</p>
            <p>#token#</p>
            <p>If you did not create an account with us, no further action is required.</p>
        </div>
        <p class=""footer"">This email was sent automatically. Please do not reply to this email.</p>
    </div>
</body>
</html>";
                    var emailContent = body.Replace("#token#", token);
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

                AppSession.SaveToken(token, email);

                // Back to LoginPage
                Frame.Navigate(typeof(VerifyResetPasswordTokenPage));
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
