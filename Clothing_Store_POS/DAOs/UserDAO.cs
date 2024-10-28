using Clothing_Store_POS.Config;
using Clothing_Store_POS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.DAOs
{
    public class UserDAO
    {
        private readonly AppDBContext _context;

        public UserDAO()
        {
            _context = new AppDBContext();
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }
    }
}
