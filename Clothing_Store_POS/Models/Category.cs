using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.Models
{
    public class Category
    {
        [Column("id")]
        [Ignore]
        public int Id { get; set; }

        [Column("name")]
        [Name("name")]
        public string Name { get; set; }
    }
}
