using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalPages { get; set; }

        public PagedResult(List<T> items, int count, int pageSize)
        {
            Items = items;
            // Math.Ceiling la`m tr`on len
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);  
        }
    }
}
