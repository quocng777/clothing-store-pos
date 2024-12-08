using Clothing_Store_POS.Config;
using Clothing_Store_POS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var list = await _context.Orders
                .Select(o => o.CreatedAt.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToListAsync();

            return list;
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
    }
}
