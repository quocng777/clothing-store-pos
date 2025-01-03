using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;

namespace Clothing_Store_POS.Models
{
    public class UserDTO : INotifyPropertyChanged
    {
#nullable enable
        [Ignore]
        public int Id { get; set; }

#nullable disable
        [Name("fullname")]
        public string UserName { get; set; }

        [Name("username")]
        public string FullName { get; set; }

        [Name("email")]
        public string Email { get; set; }

#nullable enable
        [Name("password")]
        public string? Password { get; set; }

#nullable enable
        [Ignore]
        public string? ConfirmPassword { get; set; }

#nullable disable
        [Name("user_role")]
        public string Role { get; set; }

        [Name("is_active")]
        public bool IsActive { get; set; } = true;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
