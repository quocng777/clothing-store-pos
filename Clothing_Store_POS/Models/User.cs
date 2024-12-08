using Clothing_Store_POS.Converters;
using CsvHelper.Configuration.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clothing_Store_POS.Models
{
    public class User : INotifyPropertyChanged
    {
        [Key]
        [Column("id")]
        [Ignore]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("fullname")]
        [Name("fullname")]
        public string FullName { get; set; }

        [Column("username")]
        [Name("username")]
        public string UserName { get; set; }

        [Column("password_hash")]
        [Name("password_hash")]
        public string PasswordHash { get; set; }

        [Column("email")]
        [Name("email")]
        public string Email { get; set; }

        [Column("user_role")]
        [Name("user_role")]
        public string Role { get; set; }

        [Column("is_active")]
        [Name("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_date")]
        [Name("created_date")]
        [CsvHelper.Configuration.Attributes.TypeConverter(typeof(UtcDateTimeConverter))]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(7);

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
