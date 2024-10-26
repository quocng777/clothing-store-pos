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
    }
}
