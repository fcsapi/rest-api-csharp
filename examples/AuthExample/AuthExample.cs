/*
 * FCS API - Authentication Examples
 *
 * Three authentication methods:
 * 1. access_key - Simple API key (default)
 * 2. ip_whitelist - IP whitelist (no key needed)
 * 3. token - Secure token-based (recommended for frontend)
 *
 * Get your API key at: https://fcsapi.com
 *
 * Run: dotnet run
 */

using System;
using System.Linq;
using FcsApi;

namespace AuthExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // ============================================================
            // Method 1: Simple API Key (Backward Compatible)
            // ============================================================
            Console.WriteLine("=== Method 1: Simple API Key ===\n");

            var fcsapi = new FcsApi.FcsApi("YOUR_API_KEY");
            var forex = fcsapi.Forex;
            Console.WriteLine(forex.GetLatestPrice("FX:EURUSD"));


            // ============================================================
            // Method 2: IP Whitelist (No key needed)
            // ============================================================
            Console.WriteLine("\n\n=== Method 2: IP Whitelist ===\n");

            // First, whitelist your server IP in your account:
            // https://fcsapi.com/dashboard/profile -> IP Whitelist

            var config = FcsConfig.WithIpWhitelist();
            fcsapi = new FcsApi.FcsApi(config);
            forex = fcsapi.Forex;
            Console.WriteLine(forex.GetLatestPrice("FX:EURUSD"));


            // ============================================================
            // Method 3: Token-Based Authentication (Secure)
            // ============================================================
            Console.WriteLine("\n\n=== Method 3: Token-Based (Secure) ===\n");

            // Step 1: On your BACKEND - Generate token
            config = FcsConfig.WithToken(
                "YOUR_API_KEY",      // Private key (keep secret on server)
                "YOUR_PUBLIC_KEY",   // Public key (can be exposed)
                3600                 // Token valid for 1 hour
            );

            fcsapi = new FcsApi.FcsApi(config);

            // Generate token to send to frontend
            var tokenData = fcsapi.GenerateToken();
            Console.WriteLine("Token for frontend:");
            Console.WriteLine(string.Join(", ", tokenData.Select(kv => $"{kv.Key}: {kv.Value}")));

            // This token data can be sent to frontend JavaScript:
            // {
            //     "_token": "abc123...",
            //     "_expiry": 1764164233,
            //     "_public_key": "your_public_key"
            // }

            // API request with token auth
            Console.WriteLine("\nAPI Response:");
            forex = fcsapi.Forex;
            Console.WriteLine(forex.GetLatestPrice("FX:EURUSD"));


            // ============================================================
            // Using Config Object (Advanced)
            // ============================================================
            Console.WriteLine("\n\n=== Advanced: Custom Config ===\n");

            config = new FcsConfig();
            config.AuthMethod = "access_key";
            config.AccessKey = "YOUR_API_KEY";
            config.Timeout = 60;         // Custom timeout
            config.ConnectTimeout = 10;  // Custom connect timeout

            fcsapi = new FcsApi.FcsApi(config);
            forex = fcsapi.Forex;
            Console.WriteLine(forex.GetLatestPrice("FX:EURUSD"));
        }
    }
}
