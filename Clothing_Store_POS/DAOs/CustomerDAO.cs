using Clothing_Store_POS.Config;
using Clothing_Store_POS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.DAOs
{
    public class CustomerDAO
    {
        private readonly AppDBContext _context;

        public CustomerDAO()
        {
            _context = new AppDBContext();
        }


        public List<Customer> GetAll()
        {
            return _context.Customers.ToList();
        }


        public async Task<PagedResult<Customer>> GetCustomers(int pageNumber, int pageSize, string keyword)
        {
            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query
                    .Where(p => EF.Functions.ILike(p.Name, $"%{keyword}%") || EF.Functions.ILike(p.Email, $"%{keyword}%"));
            }

            int totalItems = await query.CountAsync();

            var customers = await query
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Customer>(customers, totalItems, pageSize);
        }
    }
}
