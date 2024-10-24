using Clothing_Store_POS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.Contracts
{
    public interface IDAO<T>
    {
        Task<List<T>> GetAll();
        Task<bool> Create(T entity);
    }
}
