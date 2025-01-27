﻿using Clothing_Store_POS.DAOs;
using Clothing_Store_POS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.ViewModels
{
    public class ProductsViewModel : INotifyPropertyChanged
    {
        public readonly ProductDAO _productDAO;
        public ObservableCollection<ProductViewModel> Products { get; set; }
        public ObservableCollection<int> PageNumbers { get; set; }
        public List<int> SelectedCategoryIds { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public string Keyword { get; set; }
        public int CategoryId { get; set; }

        private int _currentPage;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        private int _perPage;
        public int PerPage
        {
            get => _perPage;
            set
            {
                _perPage = value;
                OnPropertyChanged(nameof(PerPage));
            }
        }

        public ProductsViewModel()
        {
            this._productDAO = new ProductDAO();
            CurrentPage = 1;
            PerPage = 6;
            Products = new ObservableCollection<ProductViewModel>();
            PageNumbers = new ObservableCollection<int>();
            SelectedCategoryIds = new List<int>();
        }

        public void DeleteAProduct(int productId)
        {

            ProductViewModel product = null;
            foreach (var p in Products)
            {
                if (p.Id == productId)
                {
                    product = p;
                    break;
                }
            }

            if(product == null)
            {
                return;
            }

            _productDAO.DeleteProductById(productId);
            Products.Remove(product);
            TotalItems--;   
        }

        public async Task LoadProducts(bool useNoTracking = false)
        {
            var pagedResult = await _productDAO.GetListProducts(CurrentPage, PerPage, Keyword, SelectedCategoryIds, useNoTracking);
            TotalPages = pagedResult.TotalPages;
            TotalItems = pagedResult.TotalItems;

            if (CurrentPage > TotalPages)
            {
                CurrentPage = 1;
                pagedResult = await _productDAO.GetListProducts(CurrentPage, PerPage, Keyword, SelectedCategoryIds, useNoTracking);
                TotalPages = pagedResult.TotalPages;
                TotalItems = pagedResult.TotalItems;
            }

            // update page numbers
            PageNumbers.Clear();
            for (int i = 1; i <= TotalPages; i++)
            {
                PageNumbers.Add(i);
            }

            // update products
            Products.Clear();
            foreach (var product in pagedResult.Items)
            {
                ProductViewModel productViewModel = new ProductViewModel()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    Category = product.Category,
                    Thumbnail = product.Thumbnail,
                    Sale = product.Sale,
                    Size = product.Size,
                    Stock = product.Stock,
                    IsEnabled = product.Stock > 0
                };
                Products.Add(productViewModel);
            }
        }

        public void NextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                _ = LoadProducts(true);
            }
        }

        public void PreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                _ = LoadProducts(true);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
