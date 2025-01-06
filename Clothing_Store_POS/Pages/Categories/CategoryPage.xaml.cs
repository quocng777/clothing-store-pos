using Clothing_Store_POS.Config;
using Clothing_Store_POS.Models;
using Clothing_Store_POS.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clothing_Store_POS.Pages.Categories
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CategoryPage : Page
    {
        private CategoriesViewModel _viewModel { get; }
        private FileService _fileService;
        ObservableCollection<Category> _categories;
        public string Keyword { get; set; } = string.Empty;

        public CategoryPage()
        {
            this.InitializeComponent();
            _viewModel = new CategoriesViewModel();
            _fileService = new FileService();

            // DataContext help UserPage auto find attribute of UserViewModel
            this.DataContext = _viewModel;
        }

        protected override void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is int page)
            {
                _viewModel.CurrentPage = page;
            }
            LoadCategories(null, null);
        }

        private async void LoadCategories(object sender, RoutedEventArgs e)
        {
            var tempList = await _viewModel.LoadCategories();
            _categories = new ObservableCollection<Category>(tempList);

            listCategories.ItemsSource = _categories;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreateCategoryPage), _viewModel.CurrentPage);
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var category = button?.CommandParameter as Category;

            Frame.Navigate(typeof(EditCategoryPage), new { Category = category, Page = _viewModel.CurrentPage });
        }

        private async void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.CurrentPage = 1;
            var tempList = await _viewModel.LoadCategories();
            _categories = new ObservableCollection<Category>(tempList);

            listCategories.ItemsSource = _categories;
        }

        private async void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var category = button?.CommandParameter as Category;

            var warningDialog = new ContentDialog
            {
                Title = "Warning Delete Category",
                Content = $"Are you sure you want to delete this category '{category.Name}'? All products in this category will be deleted too.\n\nThis action cannot be undone!",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot
            };

            var result = await warningDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                // Delete user 
                await _viewModel.DeleteCategory(category.Id);

                var successDialog = new ContentDialog
                {
                    Title = "Delete User",
                    Content = "User has been deleted successfully.",
                    CloseButtonText = "OK"
                };

                successDialog.XamlRoot = this.XamlRoot;

                await successDialog.ShowAsync();

                LoadCategories(null, null);
            }
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.CurrentPage > 1)
            {
                _viewModel.CurrentPage--;
                LoadCategories(null, null);
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.CurrentPage < _viewModel.TotalPages)
            {
                _viewModel.CurrentPage++;
                LoadCategories(null, null);
            }
        }

        private async void ImportCSV_Click(object sender, RoutedEventArgs e)
        {
            // Create a file picker
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            // See the sample code below for how to make the window accessible from the App class.
            var window = (Application.Current as App).MainWindow;

            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

            // Initialize the file picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            openPicker.FileTypeFilter.Add(".csv");

            Windows.Storage.StorageFile file = await openPicker.PickSingleFileAsync();
            if (file == null)
            {
                return;
            }

            var records = _fileService.ImportCsv<Category>(file.Path.ToString());

            if (records == null)
            {
                var errorDialog = new ContentDialog
                {
                    Title = "Import CSV",
                    Content = "Cannot import CSV file!",
                    CloseButtonText = "OK"
                };

                errorDialog.XamlRoot = this.XamlRoot;

                await errorDialog.ShowAsync();

                return;
            }

            foreach (var category in records)
            {
                await _viewModel.AddCategory(category);
            }

            var successDialog = new ContentDialog
            {
                Title = "Import CSV",
                Content = "Import successfully",
                CloseButtonText = "OK"
            };

            successDialog.XamlRoot = this.XamlRoot;

            await successDialog.ShowAsync();

            Frame.Navigate(typeof(CategoryPage));
        }

        private async void ExportCSV_Click(object sender, RoutedEventArgs e)
        {
            var listCategories = _categories.ToList();

            _fileService.ExportCsv<Category>(listCategories, "export_categories.csv");

            var successDialog = new ContentDialog
            {
                Title = "Export CSV",
                Content = "Please check csv file in Files folder",
                CloseButtonText = "OK"
            };

            successDialog.XamlRoot = this.XamlRoot;

            await successDialog.ShowAsync();
        }
    }
}
