using Clothing_Store_POS.Config;
using Clothing_Store_POS.DAOs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clothing_Store_POS.Pages.Customers
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SendMailPage : Page
    {
        private CustomerDAO customerDAO = new CustomerDAO();
        private EmailService emailService = new EmailService();
        private int _fromPage = 1;

        public SendMailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is int fromPage)
            {
                _fromPage = fromPage;
            }
        }

        private void CancelBtn_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Frame.Navigate(typeof(CustomerPage));
        }

        private async void SendEmail_Click(object sender, RoutedEventArgs e)
        {
            var emailSubject = mailSubjectInput.Text;
            var emailContent = mailContentInput.Text;

            if (string.IsNullOrEmpty(emailSubject) || string.IsNullOrEmpty(emailContent))
            {
                var dialog = new ContentDialog
                {
                    Title = "Errors",
                    Content = "Email Subject and Email content are required",
                    CloseButtonText = "Close",
                    XamlRoot = this.Content.XamlRoot
                };

                await dialog.ShowAsync();
                return;
            }

            var customers = customerDAO.GetAll();

            List<string> recipients = new List<string>();
            foreach (var customer in customers) {
                recipients.Add(customer.Email);
            }

            emailService.SendEmail(recipients, emailSubject, emailContent);
            var successDialog = new ContentDialog
            {
                Title = "Success",
                Content = "Email sent successfully",
                CloseButtonText = "Close",
                XamlRoot = this.Content.XamlRoot
            };

            await successDialog.ShowAsync();
            return;
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CustomerPage), _fromPage);

        }

        private async void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Warning";
            dialog.PrimaryButtonText = "OK";
            dialog.SecondaryButtonText = "Cancel";
            dialog.Content = "If your continue the processing, your temporary email data will be clear?";

            dialog.PrimaryButtonClick += CancelBtn_Click;

            await dialog.ShowAsync();
        }
    }
}
