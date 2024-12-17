using Clothing_Store_POS.Models;
using Clothing_Store_POS.Services.Statistics;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LiveChartsCore.SkiaSharpView.Extensions;
using System.Diagnostics;

namespace Clothing_Store_POS.ViewModels
{
    public class PeriodicReportViewModel : INotifyPropertyChanged
    {
        private readonly OrderService _orderService;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public ObservableCollection<UserStatsDto> TopUsers { get; set; }
        public ObservableCollection<CustomerStatsDto> TopCustomers { get; set; }
        public ObservableCollection<ProductStatsDto> TopProducts { get; set; }

        public double TotalSales { get; set; }
        public int TotalOrders { get; set; }

        public IEnumerable<ISeries> IncomeChartSeries { get; private set; } = Array.Empty<ISeries>();
        public IEnumerable<ISeries> CategorySalesSeries { get; set; } = Array.Empty<ISeries>();

        public PeriodicReportViewModel()
        {
            _orderService = new OrderService();
            FromDate = DateTime.Today;
            ToDate = DateTime.Today.AddDays(1).AddSeconds(-1);
        }

        public async Task LoadDataForDuration()
        {
            var statistics = await _orderService.GetSalesStatisticsAsync(FromDate, ToDate, 5, 5, 5);
            TotalSales = statistics.TotalSales;
            TotalOrders = statistics.TotalOrders;
            CategorySalesSeries = GenerateCategoryChart(statistics.CategorySales);
            TopUsers = new ObservableCollection<UserStatsDto>(statistics.TopUsers);
            TopCustomers = new ObservableCollection<CustomerStatsDto>(statistics.TopCustomers);
            TopProducts = new ObservableCollection<ProductStatsDto>(statistics.TopProducts);

            OnPropertyChanged(nameof(TotalSales));
            OnPropertyChanged(nameof(TotalOrders));
            OnPropertyChanged(nameof(CategorySalesSeries));
            OnPropertyChanged(nameof(TopUsers));
            OnPropertyChanged(nameof(TopCustomers));
            OnPropertyChanged(nameof(TopProducts));
        }

        private IEnumerable<ISeries> GenerateCategoryChart(List<CategorySalesDto> data)
        {
            return data
                .Select(value => value.TotalIncome)
                .AsPieSeries((value, series) =>
                {
                    var category = data.FirstOrDefault(d => d.TotalIncome == value)?.Category ?? "Unknown";
                    series.Name = category;
                    series.Values = new double[] { value };
                    series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Outer;
                    series.DataLabelsSize = 15;
                    series.DataLabelsPaint = new SolidColorPaint(SKColors.Black);

                    series.DataLabelsFormatter = point => $"{point.Coordinate.PrimaryValue:C}";

                    series.ToolTipLabelFormatter = point => $"{point.Coordinate.PrimaryValue:C}";
                });
        }

 public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
