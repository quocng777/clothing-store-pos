using Clothing_Store_POS.Config;
using Clothing_Store_POS.DAOs;
using Clothing_Store_POS.Helper;
using Clothing_Store_POS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.ViewModels
{
    public class UsersViewModel : INotifyPropertyChanged
    {
        private readonly UserDAO _userDAO;
        private FileService _fileService;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string Keyword { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public UsersViewModel()
        {
            _userDAO = new UserDAO();
            CurrentPage = 1;
            _fileService = new FileService();
        }

        public async Task<List<User>> LoadUsers()
        {
            int pageSize = 6;
            var pagedResult = await _userDAO.GetListUsers(CurrentPage, pageSize, Keyword);
            TotalPages = pagedResult.TotalPages;

            //var rootPath = _fileService.GetRootPath();
            //var users = _fileService.ImportCsv<User>($"{rootPath}/Files/CSV/users.csv");

            //foreach (var user in users)
            //{
            //    await _userDAO.AddUser(user);
            //    System.Diagnostics.Debug.WriteLine($"{user.FullName}");
            //}

            return pagedResult.Items;
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

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _userDAO.GetUserByEmail(email);

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

        public async Task<int> ResetPassword(string email, string password)
        {
            var user = await _userDAO.GetUserByEmail(email);

            user.PasswordHash = Utilities.HashPassword(password);

            var id = await _userDAO.UpdateUser(user);

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
