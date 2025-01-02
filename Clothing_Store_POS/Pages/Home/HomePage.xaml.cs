using Clothing_Store_POS.Config;
using Clothing_Store_POS.Helper;
using Clothing_Store_POS.Models;
using Clothing_Store_POS.Services.Invoice;
using Clothing_Store_POS.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Microsoft.UI.Dispatching;
using System.Threading.Tasks;
using System.Web;
using Microsoft.UI.Xaml.Navigation;
using LiveChartsCore.Kernel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clothing_Store_POS.Pages.Home
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page, INotifyPropertyChanged
    {
        public ProductViewModel ProductViewModel { get; set; }
        public ProductsViewModel ProductsViewModel { get; set; }
        public CategoriesViewModel CategoriesViewModel { get; set; }
        public CustomersViewModel CustomersViewModel { get; set; }
        public OrderViewModel OrderViewModel { get; set; }
        public ObservableCollection<CartItemViewModel> CartItems { get; set; }
        private PaymentHandler _paymentHandler;
        public double TotalAmount
        {
            get
            {
                double originalTotal = CartItems.Sum(item => item.TotalPrice);
                return originalTotal * (1 + (OrderViewModel.TaxPercentage - OrderViewModel.DiscountPercentage) / 100);
            }
        }

        private CustomerSuggestion _chosenCustomer;
        public CustomerSuggestion ChosenCustomer
        {
            get => _chosenCustomer;
            set
            {
                if (_chosenCustomer != value)
                {
                    _chosenCustomer = value;
                    OnPropertyChanged(nameof(ChosenCustomer));
                }
            }
        }

        public ComboBox DiscountTypeComboBox;
        public TextBox PercentageBox;
        public TextBox FixedBox;

        public HomePage()
        {
            _paymentHandler = new PaymentHandler();
            _paymentHandler.PaymentReceived += OnPaymentReceived;
            ProductsViewModel = new ProductsViewModel();
            ProductViewModel = new ProductViewModel();
            OrderViewModel = new OrderViewModel();
            CustomersViewModel = new CustomersViewModel();

            CartItems = [];
            CartItems.CollectionChanged += CartItems_CollectionChanged;
            this.InitializeComponent();
            //this.DataContext = this;

            PerPageComboBox.SelectedIndex = 0;
            CurrentPageComboBox.SelectedIndex = 0;
            this.NavigationCacheMode = Microsoft.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

            Task.Run(() => _paymentHandler.StartHttpListener());
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            CategoriesViewModel = new CategoriesViewModel();
            await CategoriesViewModel.InitializeAsync();
        }

        private void OnPaymentReceived(string queryString)
        {
            var queryParameters = HttpUtility.ParseQueryString(queryString);
            string responseCode = queryParameters["vnp_ResponseCode"];


            if (responseCode == "00") // Thành công
            {
                DispatcherQueue.TryEnqueue(async () => { 
                    VNPayStatusMessage.Text = "Payment completed successufully!";
                    var contentDialog = new ContentDialog
                    {
                        Title = "Alert",
                        Content = "Payment completed successufully!",
                        CloseButtonText = "Close"
                    };
                    contentDialog.XamlRoot = this.Content.XamlRoot;

                    var result = await contentDialog.ShowAsync();
                });
             
            }
            else
            {
                DispatcherQueue.TryEnqueue(async () => { 
                    VNPayStatusMessage.Text = "Payment failed. Check again!";
                    var contentDialog = new ContentDialog
                    {
                        Title = "Alert",
                        Content = "Payment failed. Check again!",
                        CloseButtonText = "Close"
                    };

                    contentDialog.XamlRoot = this.Content.XamlRoot;

                    var result = await contentDialog.ShowAsync();
                });
  
            }
        }

        // Cart actions
        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var result = true;
            var button = sender as Button;
            var product = button?.CommandParameter as Product;
            Debug.WriteLine($"Adding product to cart: {product.Name}");

            if (product == null)
            {
                Debug.WriteLine("Invalid product");
                return;
            }

            var existingCartItem = CartItems.FirstOrDefault(item => item.Product.Id == product.Id);

            if (existingCartItem != null)
            {
                result = existingCartItem.IncreaseQuantity();

                if (product.Stock == existingCartItem.Quantity)
                {
                    button.Content = "Out of Stock";
                    button.IsEnabled = false;
                }
            }
            else
            {
                CartItems.Add(new CartItemViewModel(product, 1));
            }
            OnPropertyChanged(nameof(CartItems));

            // After add to cart product having stock equals 1, set `out of stock`
            if (result == false || product.Stock == 1)
            {
                button.Content = "Out of Stock";
                button.IsEnabled = false;
            }
        }

        // handle cart items
        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is CartItemViewModel cartItem)
            {
                cartItem.IncreaseQuantity();
            }
        }

        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is CartItemViewModel cartItem)
            {
                cartItem.DecreaseQuantity();
            }
        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is CartItemViewModel cartItem)
            {
                CartItems.Remove(cartItem);
                ProductsViewModel.CurrentPage = 1;
                _ = ProductsViewModel.LoadProducts();
            }
        }

        // flyout for discount of each cart item
        private void Flyout_Opened(object sender, object e)
        {
            if (sender is Flyout flyout && flyout.Content is StackPanel panel)
            {
                if (flyout.Target is Button button && button.CommandParameter is CartItemViewModel cartItem)
                {
                    panel.DataContext = cartItem;
                }

                DiscountTypeComboBox = panel.Children.OfType<ComboBox>().FirstOrDefault();
                PercentageBox = panel.Children.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "PercentageBox");
                FixedBox = panel.Children.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "FixedBox");

                DiscountTypeComboBox.SelectedIndex = 0;
            }
        }

        private void DiscountTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DiscountTypeComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var selectedTag = selectedItem.Tag.ToString();
                PercentageBox.IsEnabled = selectedTag == "Percentage";
                FixedBox.IsEnabled = selectedTag == "Fixed";
            }
        }

        private void PercentageBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PercentageBox.IsEnabled && sender is TextBox textBox && textBox.DataContext is CartItemViewModel cartItem)
            {
                if (float.TryParse(PercentageBox.Text, out float percentage))
                {
                    double fixedDiscount = cartItem.OriginalPrice * (percentage / 100);
                    FixedBox.Text = fixedDiscount.ToString("0.00");
                }

                OnPropertyChanged(nameof(TotalAmount));
            }
        }

        private void FixedBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FixedBox.IsEnabled && sender is TextBox textBox && textBox.DataContext is CartItemViewModel cartItem)
            {
                if (double.TryParse(FixedBox.Text, out double fixedAmount))
                {
                    double percentage = (fixedAmount / cartItem.OriginalPrice) * 100;
                    PercentageBox.Text = percentage.ToString("0.00");
                }

                OnPropertyChanged(nameof(TotalAmount));
            }
        }

        private void ApplyDiscount_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is CartItemViewModel cartItem)
            {
                if(float.TryParse(PercentageBox.Text, out float discountPercentage)
                    && double.TryParse(FixedBox.Text, out double discountFixed))
                {
                    cartItem.DiscountPercentage = discountPercentage;
                    cartItem.DiscountFixed = discountFixed;
                } else
                {
                    //cartItem.DiscountPercentage = 0;
                    //cartItem.DiscountFixed = 0;
                }

                OnPropertyChanged(nameof(TotalAmount));
            }
        }

        // notify when cart items changed
        private void CartItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (CartItemViewModel item in e.NewItems)
                {
                    item.PropertyChanged += CartItem_PropertyChanged;
                }
            }
            if (e.OldItems != null)
            {
                foreach (CartItemViewModel item in e.OldItems)
                {
                    item.PropertyChanged -= CartItem_PropertyChanged;
                }
            }

            OnPropertyChanged(nameof(TotalAmount));
        }

        private void CartItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CartItemViewModel.Quantity) || e.PropertyName == nameof(CartItemViewModel.TotalPrice))
            {
                OnPropertyChanged(nameof(TotalAmount));
            }
        }

        // Product list actions
        // pagination
        private void PerPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is string selectedValue)
            {
                int perPage = int.Parse(selectedValue);
                ProductsViewModel.PerPage = perPage;
                ProductsViewModel.CurrentPage = 1;
                _ = ProductsViewModel.LoadProducts();
            }
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            ProductsViewModel.PreviousPage();
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            ProductsViewModel.NextPage();
        }

        private void Page_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is int selectedValue)
            {
                if (ProductsViewModel.PageNumbers.Contains(selectedValue))
                {
                    ProductsViewModel.CurrentPage = selectedValue;
                    _ = ProductsViewModel.LoadProducts();
                }
            }
        }

        // search
        private void SearchTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                ProductsViewModel.Keyword = SearchTextBox.Text;
                _ = ProductsViewModel.LoadProducts();
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            ProductsViewModel.CurrentPage = 1;
            _ = ProductsViewModel.LoadProducts();
        }

        // save
        private void DiscountBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (OverviewDiscountBox.Text == "")
            {
                OrderViewModel.DiscountPercentage = 0;
                OnPropertyChanged(nameof(TotalAmount));
            }
            else if (float.TryParse(OverviewDiscountBox.Text, out float percentage))
            {
                OrderViewModel.DiscountPercentage = percentage;
                OnPropertyChanged(nameof(TotalAmount));
            }
        }

        private void TaxBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (OverviewTaxBox.Text == "")
            {
                OrderViewModel.TaxPercentage = 0;
                OnPropertyChanged(nameof(TotalAmount));
            }
            else if(float.TryParse(OverviewTaxBox.Text, out float percentage))
            {
                OrderViewModel.TaxPercentage = percentage;
                OnPropertyChanged(nameof(TotalAmount));
            }
        }

        private async void SaveOrder_Click(object sender, RoutedEventArgs e)
        {
            if(CartItems.Count == 0)
            {
                return;
            }

            var dialog = new ContentDialog();
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Order saving confirmation";
            dialog.PrimaryButtonText = "OK";
            dialog.SecondaryButtonText = "Cancel";
            dialog.Content = "Do you want to save this order?";
            dialog.PrimaryButtonClick += async (s, args) =>
            {
                int customerId = ChosenCustomer != null ? ChosenCustomer.Id : -1;
                int userId = AppSession.CurrentUser.Id;
                int newOrderId = OrderViewModel.CreateOrder(customerId, userId);

                foreach (var cartItem in CartItems)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = newOrderId,
                        ProductId = cartItem.Product.Id,
                        Quantity = cartItem.Quantity,
                        DiscountPercentage = cartItem.DiscountPercentage
                    };
                    OrderViewModel.AddOrderItem(orderItem);
                    ProductViewModel.UpdateStockById(cartItem.Product.Id, cartItem.Quantity);
                }
            };

            await dialog.ShowAsync();

            CartItems.Clear();
            OrderViewModel.Note = "";
        }

        private async void SaveAndPrintOrder_Click(object sender, RoutedEventArgs e)
        {
            if (CartItems.Count == 0)
            {
                return;
            }

            var dialog = new ContentDialog();
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Saving and Printing confirmation";
            dialog.PrimaryButtonText = "OK";
            dialog.SecondaryButtonText = "Cancel";
            dialog.Content = "Do you want to save this order and print its invoice?";
            dialog.PrimaryButtonClick += async (s, args) =>
            {
                int customerId = ChosenCustomer != null ? ChosenCustomer.Id : -1;
                int userId = AppSession.CurrentUser.Id;
                int newOrderId = OrderViewModel.CreateOrder(customerId, userId);

                foreach (var cartItem in CartItems)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = newOrderId,
                        ProductId = cartItem.Product.Id,
                        Quantity = cartItem.Quantity,
                        DiscountPercentage = cartItem.DiscountPercentage
                    };
                    OrderViewModel.AddOrderItem(orderItem);
                    ProductViewModel.UpdateStockById(cartItem.Product.Id, cartItem.Quantity);
                }

                var invoiceModel = InvoiceModel.CreateInvoiceModelFromOrderId(newOrderId);
                await InvoicePrinter.GenerateAndSaveInvoice(invoiceModel);
            };

            await dialog.ShowAsync();

            CartItems.Clear();
            OrderViewModel.Note = "";
        }

        // category filter
        private void CategoryToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton toggleButton && toggleButton.DataContext is CategoryViewModel selectedCategory)
            {
                if (!ProductsViewModel.SelectedCategoryIds.Contains(selectedCategory.Id))
                {
                    ProductsViewModel.SelectedCategoryIds.Add(selectedCategory.Id);
                }

                _ = ProductsViewModel.LoadProducts();
            }
        }

        private void CategoryToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton toggleButton && toggleButton.DataContext is CategoryViewModel selectedCategory)
            {
                if (ProductsViewModel.SelectedCategoryIds.Contains(selectedCategory.Id))
                {
                    ProductsViewModel.SelectedCategoryIds.Remove(selectedCategory.Id);
                }

                _ = ProductsViewModel.LoadProducts();
            }
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Since selecting an item will also change the text,
            // only listen to changes caused by user entering text.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new List<CustomerSuggestion>();
                var splitText = sender.Text.ToLower().Split(" ");
                foreach (var customer in CustomersViewModel.Customers)
                {
                    var found = splitText.All((key) =>
                    {
                        return customer.Name.ToLower().Contains(key);
                    });
                    if (found)
                    {
                        suitableItems.Add(new CustomerSuggestion
                        {
                            Name = customer.Name,
                            Id = customer.Id
                        });
                    }
                }
                sender.ItemsSource = suitableItems;
            }
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var selectedCustomer = args.SelectedItem as CustomerSuggestion;
            if (selectedCustomer != null)
            {
                ChosenCustomer = selectedCustomer;
            }
        }

        private void OnlinePay_Click(object sender, RoutedEventArgs e)
        { 
            
            string queryString = VNPayHelper.CreatePaymentUrl(TotalAmount);
            Process.Start(new ProcessStartInfo
            {
                FileName = queryString,
                UseShellExecute = true
            });
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (CustomerToggleSwitch.IsOn)
            {
                AutoSuggestBox.IsEnabled = true;
            }
            else
            {
                if (ChosenCustomer != null)
                {
                    ChosenCustomer = null;
                }
                AutoSuggestBox.IsEnabled = false;
                AutoSuggestBox.Text = "";
                CustomerName.Text = "";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CustomerSuggestion
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public override string ToString() => $"{Name} (ID {Id})";
    }

}
