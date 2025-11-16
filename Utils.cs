

using System;
using System.Security.Cryptography;
using System.Text;
namespace walkeasyfinal.Helpers
{
   

    
  
        public static class Utils
        {
            public static string GetHash(string input, string key)
            {
                using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
                {
                    byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                }
            }
        }
    }

