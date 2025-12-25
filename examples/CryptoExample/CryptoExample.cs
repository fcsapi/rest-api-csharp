/*
 * FCS API - Crypto Example
 * Get your API key at: https://fcsapi.com
 *
 * This example only uses the Crypto module (FcsCrypto)
 * Other modules (Forex, Stock) are not loaded
 *
 * Run: dotnet run
 */

using System;
using FcsApi;

namespace CryptoExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize API - only Crypto module will be used
            var fcsapi = new FcsApi.FcsApi();

            // Access only Crypto module
            var crypto = fcsapi.Crypto;

            Console.WriteLine("=== Symbols List ===\n");
            Console.WriteLine(crypto.GetSymbolsList("crypto", "spot", "BINANCE"));

            Console.WriteLine("\n\n=== Latest Price ===\n");
            Console.WriteLine(crypto.GetLatestPrice("BINANCE:BTCUSDT"));

            Console.WriteLine("\n\n=== Historical Data ===\n");
            Console.WriteLine(crypto.GetHistory("BINANCE:BTCUSDT", "1D", 5));

            Console.WriteLine("\n\n=== Profile ===\n");
            Console.WriteLine(crypto.GetProfile("BTC"));
        }
    }
}
