using Clothing_Store_POS.Config;
using Clothing_Store_POS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.DAOs
{
    public class CategoryDAO
    {
        private readonly AppDBContext _context;

        public CategoryDAO()
        {
            _context = new AppDBContext();
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public List<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }
    }
}
