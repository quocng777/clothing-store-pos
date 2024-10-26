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
        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();

        public CategoriesViewModel()
        {
            this._categoryDAO = new CategoryDAO();
            LoadCategories();
        }

        public void LoadCategories()
        {
            var categories = _categoryDAO.GetCategories();
            this.Categories.Clear();
            foreach (var cat in categories)
            {
                this.Categories.Add(cat);
            }
        }

    }
}
