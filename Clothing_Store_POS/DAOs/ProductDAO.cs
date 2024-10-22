using Clothing_Store_POS.Config;
using Clothing_Store_POS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.DAOs
{

    public class ProductDAO
    {
        private readonly AppDBContext _context;

        public ProductDAO()
        {
            _context = AppFactory.AppDBContext;
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public List<Product> GetProducts()
        {
            Console.WriteLine(_context);
            return _context.Products.ToList();
        }
    }
}
