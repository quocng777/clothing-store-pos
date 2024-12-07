using Clothing_Store_POS.Config;
using Clothing_Store_POS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.DAOs
{
    public class UserDAO
    {
        private readonly AppDBContext _context;
        private FileService _fileService;

        public UserDAO()
        {
            _context = new AppDBContext();
            _fileService = new FileService();
        }

        public List<User> GetAllUsers()
        {
            var users = _context.Users.ToList();

            return users;
        }

        public async Task<PagedResult<User>> GetListUsers(int pageNumber, int pageSize, string keyword)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query
                    .Where(u => EF.Functions.ILike(u.Email, $"%{keyword}%") || EF.Functions.ILike(u.Id.ToString(), $"%{keyword}%"));
            }

            // Count total users
            int totalItems = await query.CountAsync();

            var users = await query.OrderBy(u => u.Id)
                                            .Skip((pageNumber - 1) * pageSize)
                                            .Take(pageSize)
                                            .ToListAsync();

            // delete passwordHash
            users.ForEach(u => u.PasswordHash = null);

            //_fileService.ExportCsv(users, "users.csv");
            _fileService.ExportPdf(users, "users.pdf");

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

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

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
            //if (string.Equals(user.FullName, existedUser.FullName))
            //{
            //    _context.Entry(existedUser).Property(u => u.FullName).IsModified = false;
            //}

            //if (string.Equals(user.UserName, existedUser.UserName))
            //{
            //    _context.Entry(existedUser).Property(u => u.UserName).IsModified = false;
            //}
            
            //if (string.Equals(user.Email, existedUser.Email))
            //{
            //    _context.Entry(existedUser).Property(u => u.Email).IsModified = false;
            //}

            _context.Entry(existedUser).Property(u => u.Id).IsModified = false;

            // Update fields 
            existedUser.FullName = user.FullName;
            existedUser.UserName = user.UserName;
            existedUser.PasswordHash = string.IsNullOrEmpty(user.PasswordHash) ? existedUser.PasswordHash : user.PasswordHash;
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

        public User FindUserById(int userId)
        {
            return _context.Users.Find(userId);
        }
    }
}
