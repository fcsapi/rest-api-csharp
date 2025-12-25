/*
 * FCS API - Stock Example
 * Get your API key at: https://fcsapi.com
 *
 * This example only uses the Stock module (FcsStock)
 * Other modules (Crypto, Forex) are not loaded
 *
 * Run: dotnet run
 */

using System;
using FcsApi;

namespace StockExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize API - only Stock module will be used
            var fcsapi = new FcsApi.FcsApi();

            // Access only Stock module
            var stock = fcsapi.Stock;

            Console.WriteLine("=== Symbols List ===\n");
            Console.WriteLine(stock.GetSymbolsList("NASDAQ"));

            Console.WriteLine("\n\n=== Latest Price ===\n");
            Console.WriteLine(stock.GetLatestPrice("NASDAQ:AAPL"));

            Console.WriteLine("\n\n=== Historical Data ===\n");
            Console.WriteLine(stock.GetHistory("NASDAQ:AAPL", "1D", 5));

            Console.WriteLine("\n\n=== Profile ===\n");
            Console.WriteLine(stock.GetProfile("AAPL"));
        }
    }
}
