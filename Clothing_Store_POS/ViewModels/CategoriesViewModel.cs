using Clothing_Store_POS.DAOs;
using Clothing_Store_POS.Helper;
using Clothing_Store_POS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.ViewModels
{
    public class CategoriesViewModel : INotifyPropertyChanged
    {
        private readonly CategoryDAO _categoryDAO;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public string Keyword { get; set; }

        public ObservableCollection<CategoryViewModel> CategoryViewModels { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public CategoriesViewModel()
        {
            _categoryDAO = new CategoryDAO();
            CurrentPage = 1;
            CategoryViewModels = new ObservableCollection<CategoryViewModel>();
        }

        public async Task InitializeAsync()
        {
            await LoadAllCategories();
        }

        public async Task LoadAllCategories()
        {
            var categories = await _categoryDAO.GetCategories();
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

        public async Task<List<Category>> LoadCategories()
        {
            int pageSize = 6;
            var pagedResult = await _categoryDAO.GetListCategories(CurrentPage, pageSize, Keyword);
            TotalPages = pagedResult.TotalPages;
            TotalItems = pagedResult.TotalItems;

            return pagedResult.Items;
        }

        public async Task<int> AddCategory(Category category)
        {
            var id = await _categoryDAO.AddCategory(category);

            return id;
        }

        public async Task<int> UpdateCategory(Category category)
        {
            var id = await _categoryDAO.UpdateCategory(category);

            return id;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var result = await _categoryDAO.DeleteCategory(id);

            return result;
        }

    }
}
