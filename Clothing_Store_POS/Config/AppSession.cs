using Clothing_Store_POS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.Config
{
    public static class AppSession
    {
        public static User CurrentUser { get; set; }
    }
}
