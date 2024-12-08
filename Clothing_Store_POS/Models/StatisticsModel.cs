using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


}
