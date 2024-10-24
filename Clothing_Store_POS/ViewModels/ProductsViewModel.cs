using Clothing_Store_POS.Contracts.DAOs;
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
        public readonly IProductDAO _productDAO;

        public ProductsViewModel(IProductDAO productDAO)
        {
            _productDAO = productDAO;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var products = await _productDAO.GetAllProducts();

            return products;
        }

    }
}
