/// <summary>
/// FCS API - Configuration
///
/// Authentication options:
/// 1. access_key - Simple API key authentication
/// 2. ip_whitelist - No key needed if IP is whitelisted in your account https://fcsapi.com/dashboard/profile
/// 3. token - Secure token-based authentication (recommended for frontend)
///
/// @package FcsApi
/// @author FCS API <support@fcsapi.com>
/// </summary>

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace FcsApi
{
    /// <summary>
    /// FCS API Configuration class
    /// Supports multiple authentication methods:
    /// - access_key: Simple API key authentication
    /// - ip_whitelist: IP whitelist (no key needed)
    /// - token: Secure token-based authentication
    /// </summary>
    public class FcsConfig
    {
        /// <summary>
        /// Authentication method: 'access_key', 'ip_whitelist', 'token'
        /// </summary>
        public string AuthMethod { get; set; } = "access_key";

        /// <summary>
        /// API Access Key (Private Key) - Get from: https://fcsapi.com/dashboard
        /// </summary>
        public string AccessKey { get; set; } = "YOUR_ACCESS_KEY_HERE";

        /// <summary>
        /// Public Key (for token-based auth) - Get from: https://fcsapi.com/dashboard
        /// </summary>
        public string PublicKey { get; set; } = "YOUR_PUBLIC_KEY_HERE";

        /// <summary>
        /// Token expiry time in seconds
        /// Options: 300 (5min), 900 (15min), 1800 (30min), 3600 (1hr), 86400 (24hr)
        /// </summary>
        public int TokenExpiry { get; set; } = 3600;

        /// <summary>
        /// Request timeout in seconds
        /// </summary>
        public int Timeout { get; set; } = 30;

        /// <summary>
        /// Connection timeout in seconds
        /// </summary>
        public int ConnectTimeout { get; set; } = 5;

        /// <summary>
        /// Create config with access_key method
        /// </summary>
        /// <param name="accessKey">Your API access key</param>
        /// <returns>FcsConfig instance</returns>
        public static FcsConfig WithAccessKey(string accessKey)
        {
            return new FcsConfig
            {
                AuthMethod = "access_key",
                AccessKey = accessKey
            };
        }

        /// <summary>
        /// Create config with IP whitelist method (no key needed)
        /// </summary>
        /// <returns>FcsConfig instance</returns>
        public static FcsConfig WithIpWhitelist()
        {
            return new FcsConfig
            {
                AuthMethod = "ip_whitelist"
            };
        }

        /// <summary>
        /// Create config with token-based authentication
        /// </summary>
        /// <param name="accessKey">Your private API key (kept on server)</param>
        /// <param name="publicKey">Your public key</param>
        /// <param name="tokenExpiry">Token validity in seconds</param>
        /// <returns>FcsConfig instance</returns>
        public static FcsConfig WithToken(string accessKey, string publicKey, int tokenExpiry = 3600)
        {
            return new FcsConfig
            {
                AuthMethod = "token",
                AccessKey = accessKey,
                PublicKey = publicKey,
                TokenExpiry = tokenExpiry
            };
        }

        /// <summary>
        /// Generate authentication token
        /// Use this on your backend, then send token to frontend
        /// </summary>
        /// <returns>Dictionary with _token, _expiry, _public_key</returns>
        public Dictionary<string, object> GenerateToken()
        {
            var expiry = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + TokenExpiry;
            var message = $"{PublicKey}{expiry}";
            var token = ComputeHmacSha256(message, AccessKey);

            return new Dictionary<string, object>
            {
                { "_token", token },
                { "_expiry", expiry },
                { "_public_key", PublicKey }
            };
        }

        /// <summary>
        /// Get authentication parameters for API request
        /// </summary>
        /// <returns>Dictionary with authentication parameters</returns>
        public Dictionary<string, object> GetAuthParams()
        {
            switch (AuthMethod)
            {
                case "ip_whitelist":
                    return new Dictionary<string, object>();
                case "token":
                    return GenerateToken();
                default: // access_key
                    return new Dictionary<string, object>
                    {
                        { "access_key", AccessKey }
                    };
            }
        }

        /// <summary>
        /// Compute HMAC-SHA256 hash
        /// </summary>
        private static string ComputeHmacSha256(string message, string secret)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secret);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            using (var hmac = new HMACSHA256(keyBytes))
            {
                var hashBytes = hmac.ComputeHash(messageBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
