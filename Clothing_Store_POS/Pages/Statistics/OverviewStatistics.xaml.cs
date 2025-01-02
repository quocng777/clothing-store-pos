using Clothing_Store_POS.Converters;
using Clothing_Store_POS.ViewModels;
using LiveChartsCore.SkiaSharpView;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Threading.Tasks;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clothing_Store_POS.Pages.Statistics
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OverviewStatistics : Page
    {
        public SalesViewModel SalesViewModel { get; set; }
        public PeriodicReportViewModel PeriodicReportViewModel { get; set; }
        private readonly List<string> Months = new()
        {
            "Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        };

        public OverviewStatistics()
        {
            this.InitializeComponent();
            SalesViewModel = new SalesViewModel();
            PeriodicReportViewModel = new PeriodicReportViewModel();
            _ = InitializeAsync();
            LoadOverviewChart();
        }

        private async Task InitializeAsync()
        {
            await SalesViewModel.LoadAvailableYearsAsync();
            await PeriodicReportViewModel.LoadDataForDuration();
        }

        private async void OnYearSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is int year)
            {
                SalesViewModel.SelectedYear = year;
                await SalesViewModel.LoadSalesDataAsync(year);
                LoadOverviewChart();
            }
        }

        private async void LoadBtn_Clicked(object sender, RoutedEventArgs e)
        {
            var fromDate = FromDatePicker.Date;
            var toDate = ToDatePicker.Date.AddDays(1).AddSeconds(-1);
            PeriodicReportViewModel.FromDate = fromDate.DateTime;
            PeriodicReportViewModel.ToDate = toDate.DateTime;
            await PeriodicReportViewModel.LoadDataForDuration();
        }

        private void LoadOverviewChart()
        {
            IncomeChart.Series = SalesViewModel.IncomeChartSeries;
            IncomeChart.XAxes = new List<Axis>
            {
                new Axis
                {
                    Labels = Months,
                    Labeler = value =>
                    {
                        int index = (int)value;
                        return index >= 0 && index < Months.Count ? Months[index] : string.Empty;
                    }
                }
            };
            IncomeChart.YAxes = new List<Axis>
            {
                new Axis
                {
                    Labeler = value => PriceToVNDConverter.ConvertToVND(value),
                }
            };
        }
    }
}
