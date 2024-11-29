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
        public static Dictionary<string, (string email, DateTime expiry)> tokenStore = new();

        public static void SaveToken(string token, string email)
        {
            // Expiry 15 minutes
            tokenStore[token] = (email, DateTime.Now.AddMinutes(15));
        }
    }
}
