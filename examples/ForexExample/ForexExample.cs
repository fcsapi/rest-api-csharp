/*
 * FCS API - Forex Example
 * Get your API key at: https://fcsapi.com
 *
 * This example only uses the Forex module (FcsForex)
 * Other modules (Crypto, Stock) are not loaded
 *
 * Run: dotnet run
 */

using System;
using FcsApi;

namespace ForexExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize API - only Forex module will be used
            var fcsapi = new FcsApi.FcsApi();

            // Access only Forex module
            var forex = fcsapi.Forex;

            Console.WriteLine("=== Symbols List ===\n");
            Console.WriteLine(forex.GetSymbolsList("forex", "spot"));

            Console.WriteLine("\n\n=== Latest Price ===\n");
            Console.WriteLine(forex.GetLatestPrice("FX:EURUSD"));

            Console.WriteLine("\n\n=== Historical Data ===\n");
            Console.WriteLine(forex.GetHistory("EURUSD", "1D", 5));

            Console.WriteLine("\n\n=== Profile ===\n");
            Console.WriteLine(forex.GetProfile("EUR"));
        }
    }
}
