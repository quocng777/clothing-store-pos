using Clothing_Store_POS.Contracts;
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
        public readonly IDAO<Product> _productDAO;

        public ProductsViewModel(IDAO<Product> productDAO)
        {
            _productDAO = productDAO;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var products = await _productDAO.GetAll();

            return products;
        }

    }
}
