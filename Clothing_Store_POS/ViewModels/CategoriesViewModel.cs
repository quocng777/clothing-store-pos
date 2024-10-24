﻿using Clothing_Store_POS.DAOs;
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

        public async void LoadCategories()
        {
            var categories = await Task.Run(() => _categoryDAO.GetCategories());
            this.Categories = new ObservableCollection<Category>(categories);
        }

    }
}
