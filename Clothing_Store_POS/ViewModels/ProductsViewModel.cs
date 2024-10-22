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
    public class ProductsViewModel
    {
        public readonly ProductDAO _productDAO;
        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();

        public ProductsViewModel()
        {
            this._productDAO = new ProductDAO();
            LoadProducts();
        }

        public async void LoadProducts()
        {
            var products = await Task.Run(() => _productDAO.GetProducts());
            Products = new ObservableCollection<Product>(products);
        }

    }
}
