using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalPages { get; set; }
        public int TotalProducts { get; set; }
        public string Keyword { get; set; }

        public DashboardViewModel()
        {
            this.TotalPages = 0;
            this.TotalProducts = 0;
            this.Keyword = "";
        }
    }
}
