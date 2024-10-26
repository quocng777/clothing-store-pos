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

        public void DeleteProductById(int productId)
        {
            var product = _context.Products.Find(productId);
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }
}
