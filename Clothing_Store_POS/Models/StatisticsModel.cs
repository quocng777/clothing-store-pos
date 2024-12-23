using System.Collections.Generic;

namespace Clothing_Store_POS.Models
{
    public class YearlySalesDto
    {
        public int Year { get; set; }
        public List<MonthlySalesDto> MonthlySales { get; set; }
        public double TotalSales { get; set; }
        public MonthlySalesDto TopPerformingMonth { get; set; }
    }

    public class MonthlySalesDto
    {
        public int Month { get; set; }
        public double TotalIncome { get; set; }
    }

    public class CategorySalesDto
    {
        public string Category { get; set; }
        public double TotalIncome { get; set; }
    }

    public class SalesStatisticsDto
    {
        public double TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public List<CategorySalesDto> CategorySales { get; set; } = new();
        public List<ProductStatsDto> TopProducts { get; set; } = new();
        public List<UserStatsDto> TopUsers { get; set; } = new();
        public List<CustomerStatsDto> TopCustomers { get; set; } = new();
    }

    public class ProductStatsDto
    {
        public string ProductName { get; set; }
        public int TotalQuantity { get; set; }
        public int Index { get; set; } = 0;
    }

    public class UserStatsDto
    {
        public string UserName { get; set; }
        public int TotalOrders { get; set; }
        public int Index { get; set; } = 0;
    }

    public class CustomerStatsDto
    {
        public string CustomerName { get; set; }
        public double TotalSpent { get; set; }
        public int Index { get; set; } = 0;
    }
}
