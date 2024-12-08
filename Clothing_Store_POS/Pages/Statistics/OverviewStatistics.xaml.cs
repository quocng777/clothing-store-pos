using Clothing_Store_POS.ViewModels;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using Microsoft.UI.Xaml.Controls;
using QuestPDF.Helpers;
using System.Collections.Generic;
using Clothing_Store_POS.Converters;
using System;
using System.Diagnostics;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clothing_Store_POS.Pages.Statistics
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OverviewStatistics : Page
    {
        public SalesViewModel ViewModel { get; set; }

        public OverviewStatistics()
        {
            this.InitializeComponent();
            ViewModel = new SalesViewModel();
            _ = ViewModel.LoadData();
            DataContext = ViewModel;
            LoadOverviewChart();
        }

        private async void OnYearSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await ViewModel.LoadSalesData(ViewModel.SelectedYear);
            LoadOverviewChart();
        }

        private void LoadOverviewChart()
        {
            IncomeChart.Series = ViewModel.IncomeChartSeries;
            IncomeChart.XAxes = new List<Axis>
            {
                new Axis
                {
                    Labels = ViewModel.Months,
                    Labeler = value =>
                    {
                        int monthIndex = (int)value;
                        if (monthIndex >= 0 && monthIndex < ViewModel.Months.Count)
                        {
                            return ViewModel.Months[monthIndex];
                        }
                        return string.Empty;
                    }
                }
            };
            IncomeChart.YAxes = new List<Axis>
            {
                new Axis
                {
                    Labeler = value => PriceToVNDConverter.convertToVND(value),
                }
            };
        }
    }
}
