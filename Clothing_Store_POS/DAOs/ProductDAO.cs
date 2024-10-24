using Clothing_Store_POS.Config;
using Clothing_Store_POS.Contracts.DAOs;
using Clothing_Store_POS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.DAOs
{

    public class ProductDAO : IProductDAO
    {
        private readonly AppDBContext _context;

        public ProductDAO(AppDBContext context)
        {
            _context = context;
        }

        //public void AddProduct(Product product)
        //{
        //    _context.Products.Add(product);
        //    _context.SaveChanges();
        //}

        public async Task<bool> Create(Product product)
        {
            var result = await _context.Products.AddAsync(product);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var products = await _context.Products.AsNoTracking()
                                                .OrderBy(p => p.Id)
                                                .ToListAsync();
            return products;
        }

        //public List<Product> GetProducts()
        //{
        //    return _context.Products.ToList();
        //}
    }
}
