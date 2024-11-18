using Clothing_Store_POS.DAOs;
using Clothing_Store_POS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.ViewModels
{
    public class CategoriesViewModel
    {
        public readonly CategoryDAO _categoryDAO;
        public ObservableCollection<CategoryViewModel> CategoryViewModels { get; set; }

        public CategoriesViewModel()
        {
            this._categoryDAO = new CategoryDAO();
            CategoryViewModels = new ObservableCollection<CategoryViewModel>();
            LoadCategories();
        }

        public void LoadCategories()
        {
            var categories = _categoryDAO.GetCategories();
            this.CategoryViewModels.Clear();
            foreach (var category in categories)
            {
                CategoryViewModels.Add(new CategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    IsSelected = false
                });
            }

        }

    }
}
