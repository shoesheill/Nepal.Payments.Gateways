using System;
using System.Security.Cryptography;
using System.Text;

namespace Nepal.Payments.Gateways
{
    /// <summary>
    /// Helper class for generating HMAC SHA256 signatures required by payment gateways.
    /// </summary>
    public static class HmacHelper
    {
        /// <summary>
        /// Generates an HMAC SHA256 signature for the given message using the provided secret key.
        /// </summary>
        /// <param name="message">The message to sign.</param>
        /// <param name="secret">The secret key for signing.</param>
        /// <returns>The Base64-encoded HMAC SHA256 signature.</returns>
        /// <exception cref="ArgumentNullException">Thrown when message or secret is null or empty.</exception>
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
    }
}
