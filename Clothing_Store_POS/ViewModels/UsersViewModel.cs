using Clothing_Store_POS.DAOs;
using Clothing_Store_POS.Helper;
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

        public async Task<User> GetUserByUsername(string username)
        {
            var user = await _userDAO.GetUserByUsername(username);

            return user;
        }

        public async Task<int> AddUser(UserDTO userDto)
        {
            var user = new User
            {
                FullName = userDto.FullName,
                UserName = userDto.UserName,
                Email = userDto.Email,
                PasswordHash = Utilities.HashPassword(userDto.Password),
                Role = userDto.Role,
                IsActive = userDto.IsActive
            };

            var id = await _userDAO.AddUser(user);

            return id;
        }

        public async Task<int> UpdateUser(UserDTO userDto)
        {
            var user = new User
            {
                Id = userDto.Id,
                FullName = userDto.FullName,
                UserName = userDto.UserName,
                Email = userDto.Email,
                PasswordHash = userDto.Password == null ? "" : Utilities.HashPassword(userDto.Password),
                Role = userDto.Role,
                IsActive = userDto.IsActive
            };

            var id = await _userDAO.UpdateUser(user);

            return id;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var result = await _userDAO.DeleteUser(id);

            return result;
        }
    }
}
