using Clothing_Store_POS.DAOs;
using Clothing_Store_POS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.ViewModels
{
    public class UsersViewModel
    {
        private readonly UserDAO _userDAO;

        public UsersViewModel()
        {
            _userDAO = new UserDAO();
        }

        public List<User> GetAllUsers()
        {
            return _userDAO.GetAllUsers();
        }
    }
}
