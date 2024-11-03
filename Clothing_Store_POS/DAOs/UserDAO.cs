using Clothing_Store_POS.Config;
using Clothing_Store_POS.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<PagedResult<User>> GetListUsers(int pageNumber, int pageSize)
        {
            // Count total users
            int totalItems = await _context.Users.CountAsync();

            var users = await _context.Users.OrderBy(u => u.Id)
                                            .Skip((pageNumber - 1) * pageSize)
                                            .Take(pageSize)
                                            .ToListAsync();

            return new PagedResult<User>(users, totalItems, pageSize);
        }

        //public async Task<PagedResult<User>> SearchFilter(string search, int pageNumber, int pageSize)
        //{

        //}

        public async Task<User> GetUserByUsername(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

            return user;
        }

        public async Task<int> AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user.Id;
        }

        public async Task<int> UpdateUser(User user)
        {
            var existedUser = await _context.Users.FindAsync(user.Id);

            // Neu 2 value giong nhau thi isModified == false
            if (string.Equals(user.FullName, existedUser.FullName))
            {
                _context.Entry(existedUser).Property(u => u.FullName).IsModified = false;
            }

            if (string.Equals(user.UserName, existedUser.UserName))
            {
                _context.Entry(existedUser).Property(u => u.UserName).IsModified = false;
            }

            if (string.Equals(user.Email, existedUser.Email))
            {
                _context.Entry(existedUser).Property(u => u.Email).IsModified = false;
            }

            _context.Entry(existedUser).Property(u => u.Id).IsModified = false;
            //_context.Entry(existedUser).Property(u => u.CreatedDate).IsModified = false;

            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                _context.Entry(existedUser).Property(u => u.PasswordHash).IsModified = false;
            }

            // Update fields 
            existedUser.FullName = user.FullName;
            existedUser.UserName = user.UserName;
            existedUser.PasswordHash = user.PasswordHash;
            existedUser.Email = user.Email;
            existedUser.Role = user.Role;
            existedUser.IsActive = user.IsActive;
            existedUser.CreatedDate = user.CreatedDate;

            await _context.SaveChangesAsync();

            return existedUser.Id;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
