using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clothing_Store_POS.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        [Ignore]
        public int Id { get; set; }

        [Column("name")]
        [Name("name")]
        public string Name { get; set; }

        [Column("price")]
        [Name("price")]
        public double Price { get; set; }

        [ForeignKey("Category")]
        [Column("category_id")]
        [Name("category_id")]
        public int CategoryId { get; set; }
        
        [Ignore]
        public virtual Category Category { get; set; }

        [Column("size")]
        [Name("size")]
        public string Size { get; set; }

        [Column("color")]
        [Name("color")]
        public string Color { get; set; }

        [Column("stock")]
        [Name("stock")]
        public int Stock { get; set; }

        [Column("sale")]
        [Name("sale")]
        public float Sale { get; set; }

        [Column("thumbnail")]
        [Name("thumbnail")]
        public string Thumbnail { get; set; }
    }
}
