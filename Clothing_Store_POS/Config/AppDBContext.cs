using Clothing_Store_POS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.Config
{
    public class AppDBContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.ToTable("categories");
            });

            modelBuilder.Entity<Product>()
            .ToTable("products");

            modelBuilder.Entity<User>().ToTable("users");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=pos_db;Username=admin;Password=admin123");
        }

        public void InitializeDatabase()
        {
            // Tự động tạo bảng nếu chưa tồn tại
            Database.EnsureCreated();
        }
    }
}
