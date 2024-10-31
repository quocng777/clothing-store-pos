using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.Models
{
    public class UserDTO : INotifyPropertyChanged
    {
#nullable enable
        public int Id { get; set; }

#nullable disable
        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

#nullable enable
        public string? Password { get; set; }

#nullable enable
        public string? ConfirmPassword { get; set; }

#nullable disable
        public string Role { get; set; }

        public bool IsActive { get; set; } = true;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
