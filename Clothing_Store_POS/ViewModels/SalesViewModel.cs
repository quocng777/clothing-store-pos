using Clothing_Store_POS.Models;
using Clothing_Store_POS.Services.Statistics;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using QuestPDF.Helpers;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clothing_Store_POS.Converters;
using System.Diagnostics;

namespace Clothing_Store_POS.ViewModels
{
    public class SalesViewModel : INotifyPropertyChanged
    {
        private readonly OrderService _orderService;
        private YearlySalesDto _yearlySales;
        private List<int> _availableYears;
        private int _selectedYear;

        public List<int> AvailableYears
        {
            get => _availableYears;
            set
            {
                _availableYears = value;
                OnPropertyChanged(nameof(AvailableYears));
            }
        }

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

        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
                _ = LoadSalesData(value);
            }
        }

        public ISeries[] IncomeChartSeries { get; set; }
        public List<string> Months { get; set; }

        public SalesViewModel()
        {
            _orderService = new OrderService();
            AvailableYears = new List<int>();
            Months = new List<string>
            {
                "Jan", "Feb", "Mar", "Apr", "May", "Jun",
                "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
            };
        }

        public async Task LoadSalesData(int year)
        {
            YearlySales = await _orderService.GetYearlySalesAsync(year);
        }

        public async Task LoadAvailableYears()
        {
            var years = await _orderService.GetAvailableYearsAsync();
            AvailableYears = years;
        }

        public async Task LoadData()
        {
            await LoadAvailableYears();
            if (AvailableYears.Count > 0)
            {
                SelectedYear = AvailableYears[0];
            }
        }

        private void PrepareChartData()
        {
            var totalSales = YearlySales.MonthlySales.Select(ms => ms.TotalIncome).ToArray();
            IncomeChartSeries = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Values = totalSales,
                    Fill = new SolidColorPaint(SKColors.Blue),
                    Name = "Monthly Sales"
                }
            };
        }

        public string YAxisFormatter(double value)
        {
            return PriceToVNDConverter.convertToVND(value);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
