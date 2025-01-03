using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;

namespace Clothing_Store_POS.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        [Ignore]
        public int Id { get; set; }

        [Column("name")]
        [Name("name")]
        public string Name { get; set; }

        [Column("email")]
        [Name("email")]
        public string Email { get; set; }

        [Column("phone")]
        [Name("phone")]
        public string Phone { get; set; }

    }
}
