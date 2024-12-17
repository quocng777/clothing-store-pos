using Clothing_Store_POS.Models;
using Clothing_Store_POS.Services.Statistics;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Clothing_Store_POS.ViewModels
{
    public class SalesViewModel : INotifyPropertyChanged
    {
        private readonly OrderService _orderService;
        private YearlySalesDto _yearlySales;
        public List<int> AvailableYears { get; set; } = new();
        public int SelectedYear { get; set; }

        public YearlySalesDto YearlySales
        {
            get => _yearlySales;
            set
            {
                _yearlySales = value;
                OnPropertyChanged(nameof(YearlySales));
                PrepareChartData();
            }
        }

        public IEnumerable<ISeries> IncomeChartSeries { get; private set; } = Array.Empty<ISeries>();

        public SalesViewModel()
        {
            _orderService = new OrderService();
        }

        public async Task LoadSalesDataAsync(int year)
        {
            YearlySales = await _orderService.GetYearlySalesAsync(year);
        }

        public async Task LoadAvailableYearsAsync()
        {
            AvailableYears = await _orderService.GetAvailableYearsAsync();
            if (AvailableYears.Count > 0)
            {
                SelectedYear = AvailableYears[0];
            }
            else
            {
                SelectedYear = DateTime.Now.Year;
            }
        }

        private void PrepareChartData()
        {
            if (YearlySales?.MonthlySales == null)
            {
                IncomeChartSeries = Array.Empty<ISeries>();
                OnPropertyChanged(nameof(IncomeChartSeries));
                return;
            }

            IncomeChartSeries = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Values = YearlySales.MonthlySales.Select(ms => ms.TotalIncome).ToArray(),
                    Fill = new SolidColorPaint(SKColors.BlueViolet),
                    Name = "Sales"
                }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
