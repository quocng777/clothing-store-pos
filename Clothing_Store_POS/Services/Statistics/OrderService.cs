using Clothing_Store_POS.Config;
using Clothing_Store_POS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Clothing_Store_POS.Services.Statistics
{
    public class OrderService
    {
        private readonly AppDBContext _context;
        public OrderService()
        {
            _context = new AppDBContext();
        }

        public async Task<List<int>> GetAvailableYearsAsync()
        {
            return await _context.Orders
                .Select(o => o.CreatedAt.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToListAsync();
        }

        public async Task<YearlySalesDto> GetYearlySalesAsync(int year)
        {
            var orders = await _context.Orders
                .Where(o => o.CreatedAt.Year == year)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();

            var monthlySales = new List<MonthlySalesDto>();

            for (int month = 1; month <= 12; month++)
            {
                var monthlyIncome = orders
                    .Where(o => o.CreatedAt.Month == month)
                    .SelectMany(o => o.OrderItems)
                    .Sum(oi => oi.Quantity * oi.Product.Price * (1 - oi.DiscountPercentage / 100));

                monthlySales.Add(new MonthlySalesDto
                {
                    Month = month,
                    TotalIncome = monthlyIncome
                });
            }

            var totalSales = monthlySales.Sum(ms => ms.TotalIncome);

            var topPerformingMonth = monthlySales
                .OrderByDescending(ms => ms.TotalIncome)
                .FirstOrDefault();

            return new YearlySalesDto
            {
                Year = year,
                MonthlySales = monthlySales,
                TotalSales = totalSales,
                TopPerformingMonth = topPerformingMonth
            };
        }

        public async Task<SalesStatisticsDto> GetSalesStatisticsAsync(DateTime fromDate, DateTime toDate, int takenProducts, int takenUsers, int takenCustomers)
        {
            var orders = await FetchOrdersAsync(fromDate, toDate);

            var totalSales = CalculateTotalSales(orders);
            var totalOrders = CalculateTotalOrders(orders);

            var categorySales = await CalculateCategorySalesAsync(orders);
            var topProducts = GetTopProducts(orders, takenProducts);
            for (int i = 0; i < topProducts.Count; i++)
            {
                topProducts[i].Index = i + 1;
            }
            var topUsers = GetTopUsers(orders, takenUsers);
            for (int i = 0; i < topUsers.Count; i++)
            {
                topUsers[i].Index = i + 1;
            }
            var topCustomers = GetTopCustomers(orders, takenCustomers);
            for (int i = 0; i < topCustomers.Count; i++)
            {
                topCustomers[i].Index = i + 1;
            }

            return new SalesStatisticsDto
            {
                TotalSales = totalSales,
                TotalOrders = totalOrders,
                CategorySales = categorySales,
                TopProducts = topProducts,
                TopUsers = topUsers,
                TopCustomers = topCustomers
            };
        }

        private async Task<List<Order>> FetchOrdersAsync(DateTime fromDate, DateTime toDate)
        {
            DateTime utcFromDate = fromDate.ToUniversalTime();
            DateTime utcToDate = toDate.ToUniversalTime();
            
            try
            {
                var list = await _context.Orders
                    .Where(o => o.CreatedAt >= utcFromDate && o.CreatedAt <= utcToDate)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .ThenInclude(p => p.Category)
                    .Include(o => o.User)
                    .ToListAsync();

                return list;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[OrderService] Exception: " + ex.Message);
                throw;
            }
        }

        private double CalculateTotalSales(List<Order> orders)
        {
            return orders
                .SelectMany(o => o.OrderItems)
                .Sum(oi => oi.Quantity * oi.Product.Price * (1 - oi.DiscountPercentage / 100));
        }

        private int CalculateTotalOrders(List<Order> orders)
        {
            return orders.Count;
        }

        private async Task<List<CategorySalesDto>> CalculateCategorySalesAsync(List<Order> orders)
        {
            return await Task.FromResult(orders
                .SelectMany(o => o.OrderItems)
                .GroupBy(oi => oi.Product.Category.Name)
                .Select(g => new CategorySalesDto
                {
                    Category = g.Key,
                    TotalIncome = g.Sum(oi => oi.Quantity * oi.Product.Price * (1 - oi.DiscountPercentage / 100))
                })
                .ToList());
        }

        private List<ProductStatsDto> GetTopProducts(List<Order> orders, int takeNumber)
        {
            return orders
                .SelectMany(o => o.OrderItems)
                .GroupBy(oi => oi.Product.Name)
                .Select(g => new ProductStatsDto
                {
                    ProductName = g.Key,
                    TotalQuantity = g.Sum(oi => oi.Quantity),
                })
                .OrderByDescending(x => x.TotalQuantity)
                .Take(takeNumber)
                .ToList();
        }

        private List<UserStatsDto> GetTopUsers(List<Order> orders, int takeNumber)
        {
            return orders
                .GroupBy(o => o.User)
                .Select(g => new UserStatsDto
                {
                    UserName = g.Key.FullName,
                    TotalOrders = g.Count(),
                })
                .OrderByDescending(x => x.TotalOrders)
                .Take(takeNumber)
                .ToList();
        }

        private List<CustomerStatsDto> GetTopCustomers(List<Order> orders, int takeNumber)
        {
            return orders
                .SelectMany(o => o.OrderItems)
                .GroupBy(oi => oi.Order.Customer)
                .Select(g => new CustomerStatsDto
                {
                    CustomerName = g.Key?.Name,
                    TotalSpent = g.Sum(oi => oi.Quantity * oi.Product.Price * (1 - oi.DiscountPercentage / 100)),
                })
                .OrderByDescending(x => x.TotalSpent)
                .Take(takeNumber)
                .ToList();
        }
    }
}
