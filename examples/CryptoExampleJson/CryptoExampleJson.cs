/*
 * FCS API - Crypto Example
 * Get your API key at: https://fcsapi.com
 */

using System;
using System.Collections.Generic;
using System.Text.Json;
using FcsApi;

namespace FcsApiExamples
{
    class CryptoExample
    {
        static void Main(string[] args)
        {
            // Initialize API
            var fcsapi = new FcsApi.FcsApi();
            var crypto = fcsapi.Crypto;

            Console.WriteLine("=== Symbols List ===\n");
            var symbolsList = crypto.GetSymbolsList("crypto", "spot", "BINANCE");
            PrintJson(symbolsList);

            Console.WriteLine("\n\n=== Latest Price ===\n");
            var latestPrice = crypto.GetLatestPrice("BINANCE:BTCUSDT");
            PrintJson(latestPrice);

            Console.WriteLine("\n\n=== Historical Data ===\n");
            var history = crypto.GetHistory("BINANCE:BTCUSDT", "1D", 5);
            PrintJson(history);

            Console.WriteLine("\n\n=== Profile ===\n");
            var profile = crypto.GetProfile("BTC");
            PrintJson(profile);
        }

        // Helper method to print JSON
        static void PrintJson(Dictionary<string, object> data)
        {
            if (data != null)
            {
                var options = new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                };
                string json = JsonSerializer.Serialize(data, options);
                Console.WriteLine(json);
            }
            else
            {
                Console.WriteLine("No data received");
            }
        }
    }
}