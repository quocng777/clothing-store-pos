using Clothing_Store_POS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.Contracts.DAOs
{
    public interface IProductDAO
    {
        Task<List<Product>> GetAllProducts();
        Task<bool> Create(Product product);
    }
}
