using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Clothing_Store_POS.Pages.Products;
using Clothing_Store_POS.Pages.Users;
using Clothing_Store_POS.Pages.Home;
using Clothing_Store_POS.Pages.Auth;
using Clothing_Store_POS.Pages.Customers;
using Clothing_Store_POS.Pages.Orders;
using Clothing_Store_POS.Config;
using Clothing_Store_POS.Pages.Statistics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clothing_Store_POS.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainLayout : Page
    {
        public MainLayout()
        {
            this.InitializeComponent();
            ConfigureNavigationView();
            this.MainContent.Navigate(typeof(HomePage));
        }

        // handle page selected event
        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            string selectedTag = args.InvokedItemContainer.Tag.ToString();

            switch (selectedTag) {
                case "home":
                    this.MainContent.Navigate(typeof(HomePage));
                    break;
                case "statistics":
                    this.MainContent.Navigate(typeof(OverviewStatistics));
                    break;
                case "products":
                    this.MainContent.Navigate(typeof(ProductPage));
                    break;
                case "users":
                    this.MainContent.Navigate(typeof(UserPage));
                    break;
                case "customers":;
                    this.MainContent.Navigate(typeof(CustomerPage));
                    break;
                case "orders":
                    this.MainContent.Navigate(typeof(OrderPage));
                    break;
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        // Authorization
        private void ConfigureNavigationView()
        {
            var role = AppSession.CurrentUser?.Role;

            if (role == "staff")
            {
                // Allow only Home & Customers
                navigation_bar.MenuItems.Clear();
                navigation_bar.MenuItems.Add(new NavigationViewItem { Content = "Home", Icon = new SymbolIcon(Symbol.Home), Tag = "home" });
                navigation_bar.MenuItems.Add(new NavigationViewItem { Content = "Statistics", Icon = new SymbolIcon(Symbol.ThreeBars), Tag = "statistics" });
                navigation_bar.MenuItems.Add(new NavigationViewItem { Content = "Orders", Icon = new FontIcon { Glyph = "\uE719" }, Tag = "orders" });
                navigation_bar.MenuItems.Add(new NavigationViewItem { Content = "Customer", Icon = new SymbolIcon(Symbol.Mail), Tag = "customers" });
            }
            else if (role == "admin")
            {
                navigation_bar.MenuItems.Clear();
                navigation_bar.MenuItems.Add(new NavigationViewItem { Content = "Home", Icon = new SymbolIcon(Symbol.Home), Tag = "home" });
                navigation_bar.MenuItems.Add(new NavigationViewItem { Content = "Statistics", Icon = new SymbolIcon(Symbol.ThreeBars), Tag = "statistics" });
                navigation_bar.MenuItems.Add(new NavigationViewItem { Content = "Products", Icon = new BitmapIcon { UriSource = new Uri("ms-appx:///Assets/clothing_icon.png") }, Tag = "products" });
                navigation_bar.MenuItems.Add(new NavigationViewItem { Content = "Orders", Icon = new FontIcon { Glyph = "\uE719" }, Tag = "orders" });
                navigation_bar.MenuItems.Add(new NavigationViewItem { Content = "Users", Icon = new SymbolIcon(Symbol.Contact), Tag = "users" });
                navigation_bar.MenuItems.Add(new NavigationViewItem { Content = "Customer", Icon = new SymbolIcon(Symbol.Mail), Tag = "customers" });
            }
        }
    }
}
