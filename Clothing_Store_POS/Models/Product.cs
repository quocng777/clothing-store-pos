using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.Models
{
    public class Product : INotifyPropertyChanged
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [ForeignKey("Category")]
        [Column("category_id")]
        public int CategoryId { get; set; }

        [Column("size")]
        public string Size { get; set; }

        [Column("color")]
        public string Color { get; set; }

        [Column("stock")]
        public int Stock { get; set; }

        [Column("sale")]
        public float Sale { get; set; }
#nullable enable
        public virtual Category? Category { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
