using Clothing_Store_POS.Config;
using Clothing_Store_POS.Models;
using iText.Layout.Element;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Category>> GetCategories(bool useNoTracking = false)
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<PagedResult<Category>> GetListCategories(int pageNumber, int pageSize, string keyword, bool useNoTracking = false)
        {
            var query = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query
                    .Where(u => EF.Functions.ILike(u.Name, $"%{keyword}%") || EF.Functions.ILike(u.Id.ToString(), $"%{keyword}%"));
            }

            if (useNoTracking)
            {
                query = query.AsNoTracking();
            }

            // Count total categories
            int totalItems = await query.CountAsync();

            var categories = await query.OrderBy(u => u.Id)
                                            .Skip((pageNumber - 1) * pageSize)
                                            .Take(pageSize)
                                            .ToListAsync();

            return new PagedResult<Category>(categories, totalItems, pageSize);
        }
        public async Task<int> AddCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return category.Id;
        }

        public async Task<int> UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return category.Id;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }

        public Category FindCategoryById(int catId)
        {
            return _context.Categories.Find(catId);
        }
    }
}
