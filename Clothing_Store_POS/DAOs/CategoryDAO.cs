using Clothing_Store_POS.Config;
using Clothing_Store_POS.Contracts;
using Clothing_Store_POS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.DAOs
{
    public class CategoryDAO : IDAO<Category>
    {
        private readonly AppDBContext _context;

        public CategoryDAO()
        {
            _context = null;
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public Task<bool> Create(Category entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Category>> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }
    }
}
