using System;
using System.Security.Cryptography;
using System.Text;

namespace payment_gateway_nepal
{
    public static class HmacHelper
    {
        public static string GenerateHmacSha256Signature(string message, string secret)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(secret);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            using (var hmacsha256 = new HMACSHA256(keyBytes))
            {
                byte[] hashBytes = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
