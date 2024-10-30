using System;
using System.Text;
using BCrypt.Net;

namespace Clothing_Store_POS.Helper
{
    public static class Utilities
    {
        public static string GetRandomKey(int length = 5)
        {
            // pattern
            string pattern = @"0123456789zxcvbnmasdfghjklqwertyuiop[]{}:~!@#$%^&*()+";
            Random rd = new Random();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                sb.Append(pattern[rd.Next(0, pattern.Length)]);
            }

            return sb.ToString();
        }

        public static string HashPassword(string password)
        {
            // Hash password with salt created automatically
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Get salt from hashedPassword & check current passwork\d
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
