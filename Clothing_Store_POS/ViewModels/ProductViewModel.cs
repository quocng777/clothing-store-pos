using Clothing_Store_POS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Clothing_Store_POS.DAOs;
using System.IO;
using Microsoft.UI.Xaml.Controls;

namespace Clothing_Store_POS.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private readonly ProductDAO _productDAO;

        public ProductViewModel()
        {
            this._productDAO = new ProductDAO();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public string Size { get; set; }
        public int Stock { get; set; }
        public float Sale { get; set; }
        public string Thumbnail { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Save()
        {
            // handle saving image
            if (Thumbnail != null)
            {
                Thumbnail = SaveThumbnailImage(Thumbnail);
            }

            var product = new Product
            {
                Name = this.Name,
                CategoryId = this.CategoryId,
                Price = this.Price,
                Size = this.Size,
                Stock = this.Stock,
                Sale = this.Sale,
                Thumbnail = this.Thumbnail
            };

            return this._productDAO.AddProduct(product);
        }

        private String SaveThumbnailImage(String path)
        {
            string origin = path;
            string desFolderPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Images");
            Directory.CreateDirectory(desFolderPath);
            var fileExt = Path.GetExtension(path);
            var newFileName = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString() + fileExt;
            string desPath = Path.Combine(desFolderPath, newFileName);

            File.Copy(path, desPath, true);

            return desPath;
        }

        public void Update()
        {
            var savedProduct = _productDAO.findProductById(this.Id);
            if (savedProduct == null)
            {
                throw new Exception("Product not found");
            }

            savedProduct.Name = this.Name;
            savedProduct.CategoryId = this.CategoryId;
            savedProduct.Price = this.Price;
            savedProduct.Size = this.Size;
            savedProduct.Stock = this.Stock;
            savedProduct.Sale = this.Sale;

            if(this.Thumbnail != savedProduct.Thumbnail && this.Thumbnail != null)
            {
                savedProduct.Thumbnail = SaveThumbnailImage(Thumbnail);
            }

            _productDAO.UpdateProduct(savedProduct);
        }

        public void Clear()
        {
            Name = String.Empty;
            CategoryId = 0;
            Price = 0;
            Size = String.Empty;
            Stock = 0;
            Sale = 0;
            Thumbnail = String.Empty;
        }

    }
}
