using Clothing_Store_POS.DAOs;
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
    public class ProductsViewModel : INotifyPropertyChanged
    {
        public readonly ProductDAO _productDAO;
        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string Keyword { get; set; } = "";

        public event PropertyChangedEventHandler PropertyChanged;

        public ProductsViewModel()
        {
            this._productDAO = new ProductDAO();
            LoadProducts(1, 6);
        }

        public async void LoadProducts()
        {
            var products = await Task.Run(_productDAO.GetProducts);
            Products.Clear();
            foreach (var product in products) {
                Products.Add(product);
            }
        }

        public void DeleteAProduct(int productId)
        {

            Product product = null;
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
        }

        public async void LoadProducts(int pageNumber = 1, int pageSize = 10)
        {
            var pagedResult = await _productDAO.GetListUsers(pageNumber, pageSize, Keyword);
            TotalPages = pagedResult.TotalPages;
            CurrentPage = pageNumber;

            Products.Clear();
            foreach (var product in pagedResult.Items)
            {
                Products.Add(product);
            }
        }
    }
}
