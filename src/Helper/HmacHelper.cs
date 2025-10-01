using System;
using System.Security.Cryptography;
using System.Text;

namespace Nepal.Payments.Gateways.Helper
{
    public static class HmacHelper
    {
        public static string GenerateHmacSha256Signature(string message, string secret)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message), "Message cannot be null or empty.");
            
            if (string.IsNullOrEmpty(secret))
                throw new ArgumentNullException(nameof(secret), "Secret key cannot be null or empty.");

            byte[] keyBytes = Encoding.UTF8.GetBytes(secret);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            using (var hmacsha256 = new HMACSHA256(keyBytes))
            {
                byte[] hashBytes = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public static string GenerateHmacSha256(string message, string secret)
        {
            return GenerateHmacSha256Signature(message, secret);
        }

        public static string GenerateHmacSha512(string message, string secret)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message), "Message cannot be null or empty.");
            
            if (string.IsNullOrEmpty(secret))
                throw new ArgumentNullException(nameof(secret), "Secret key cannot be null or empty.");

            byte[] keyBytes = Encoding.UTF8.GetBytes(secret);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            using (var hmacsha512 = new HMACSHA512(keyBytes))
            {
                byte[] hashBytes = hmacsha512.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
