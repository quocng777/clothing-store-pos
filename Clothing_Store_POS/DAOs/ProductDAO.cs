using Clothing_Store_POS.Config;
using Clothing_Store_POS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Gaming.Input.ForceFeedback;

namespace Clothing_Store_POS.DAOs
{

    public class ProductDAO
    {
        private readonly AppDBContext _context;

        public ProductDAO()
        {
            _context = new AppDBContext();
        }

        public int AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();


            return product.Id;
        }

        public List<Product> GetProducts()
        {
            return _context.Products.ToList();
        }

        public async Task<PagedResult<Product>> GetListProducts(int pageNumber, int pageSize, string keyword, int categoryId = 0)
        {
            var query = _context.Products.AsQueryable();

            if (categoryId != 0)
            {
                query = query
                    .Where(p => p.CategoryId == categoryId);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query
                    .Where(p => EF.Functions.ILike(p.Name, $"%{keyword}%") || EF.Functions.ILike(p.Id.ToString(), $"%{keyword}%"));
            }
            
            int totalItems = await query.CountAsync();
            
            var products = await query
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(p => p.Category)
                .ToListAsync();

            return new PagedResult<Product>(products, totalItems, pageSize);
        }

        public void DeleteProductById(int productId)
        {
            var product = _context.Products.Find(productId);
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public Product findProductById(int productId)
        {
            return _context.Products.Find(productId);
        }
    }
}
