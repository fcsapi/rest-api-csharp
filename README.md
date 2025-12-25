# FCSAPI - C# REST Client

**C#** REST API client library for **Forex**, **Cryptocurrency**, and **Stock** market data from [FCS API](https://fcsapi.com).

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET Version](https://img.shields.io/badge/.NET-6.0%20|%208.0%20|%2010.0-blue.svg)](https://dotnet.microsoft.com)

## Features

- **Forex API** - 4000+ currency pairs, real-time rates, commodities, historical data, technical analysis
- **Crypto API** - 50,000+ coins from major exchanges (Binance, Coinbase, etc.), market cap, rank, coin data
- **Stock API** - 125,000+ global stocks, indices, earnings, financials, dividends
- **Easy to Use** - Simple method calls for all API endpoints
- **Multiple Auth Methods** - API key, IP whitelist, or secure token-based authentication
- **Multi-Target** - Supports .NET 6.0, .NET 8.0, and .NET 10.0

## Installation

### Clone Repository
```bash
git clone https://github.com/fcsapi/rest-api-csharp
```

### Or Download
Download ZIP from [GitHub](https://github.com/fcsapi/rest-api-csharp) and extract to your project.

```csharp
using FcsApi;
```

## Quick Start

```csharp
using FcsApi;

var fcsapi = new FcsApi.FcsApi();

// Forex
var response = fcsapi.Forex.GetLatestPrice("EURUSD");

// Crypto
response = fcsapi.Crypto.GetLatestPrice("BINANCE:BTCUSDT");

// Stock
response = fcsapi.Stock.GetLatestPrice("NASDAQ:AAPL");
```

## Authentication Methods

The library supports 4 authentication methods for different security needs:

### Method 1: Default Configuration (Recommended)
Set your API key once in `src/FcsConfig.cs`:
```csharp
public string AccessKey { get; set; } = "YOUR_API_KEY_HERE";
```
Then simply use:
```csharp
var fcsapi = new FcsApi.FcsApi();
```

### Method 2: Direct API Key
Pass API key directly (overrides config):
```csharp
var fcsapi = new FcsApi.FcsApi("YOUR_API_KEY");
```

### Method 3: IP Whitelist (No Key Required)
Whitelist your server IP at [FCS Dashboard](https://fcsapi.com/dashboard/profile):
```csharp
var config = FcsConfig.WithIpWhitelist();
var fcsapi = new FcsApi.FcsApi(config);
```

### Method 4: Token-Based Authentication (Secure for Frontend)
Generate secure tokens on backend, use on frontend without exposing API key:
```csharp
// Backend: Generate token
var config = FcsConfig.WithToken("YOUR_API_KEY", "YOUR_PUBLIC_KEY", 3600);
var fcsapi = new FcsApi.FcsApi(config);
var tokenData = fcsapi.GenerateToken();
// Returns: {"_token": "...", "_expiry": 1234567890, "_public_key": "..."}

// Send tokenData to frontend for secure API calls
```

**Token Expiry Options:**
| Seconds | Duration |
|---------|----------|
| 300 | 5 minutes |
| 900 | 15 minutes |
| 1800 | 30 minutes |
| 3600 | 1 hour |
| 86400 | 24 hours |

## Project Structure

```
rest-api-csharp/
├── src/
│   ├── FcsApi.csproj          # Main library project
│   ├── FcsApi.cs              # Main API client
│   ├── FcsConfig.cs           # Configuration & authentication
│   ├── FcsForex.cs            # Forex module
│   ├── FcsCrypto.cs           # Crypto module
│   └── FcsStock.cs            # Stock module
├── examples/
│   ├── CryptoExample/         # Crypto API example
│   ├── ForexExample/          # Forex API example
│   ├── StockExample/          # Stock API example
│   └── AuthExample/           # Authentication examples
├── README.md
├── FUNCTIONS.md
└── LICENSE
```

## Running Examples

Each example has its own project file. To run an example:

```bash
cd examples/CryptoExample
dotnet run

cd examples/ForexExample
dotnet run

cd examples/StockExample
dotnet run

cd examples/AuthExample
dotnet run
```

## API Reference

### Forex API

```csharp
// ==================== Symbol List ====================
fcsapi.Forex.GetSymbolsList();                    // All symbols
fcsapi.Forex.GetSymbolsList("forex");             // Forex only
fcsapi.Forex.GetSymbolsList("commodity");         // Commodities only

// ==================== Latest Prices ====================
fcsapi.Forex.GetLatestPrice("EURUSD");
fcsapi.Forex.GetLatestPrice("EURUSD,GBPUSD,USDJPY");
fcsapi.Forex.GetLatestPrice("EURUSD", "1D", null, true);  // with profile
fcsapi.Forex.GetAllPrices("FX");                  // All from exchange

// ==================== Commodities ====================
fcsapi.Forex.GetCommodities();                    // All commodities
fcsapi.Forex.GetCommodities("XAUUSD");           // Gold
fcsapi.Forex.GetCommoditySymbols();              // Commodity symbols list

// ==================== Currency Converter ====================
fcsapi.Forex.Convert("EUR", "USD", 100);         // Convert 100 EUR to USD

// ==================== Base Currency ====================
fcsapi.Forex.GetBasePrices("USD");               // USD to all currencies

// ==================== Cross Rates ====================
fcsapi.Forex.GetCrossRates("USD", "forex", "1D");

// ==================== Historical Data ====================
fcsapi.Forex.GetHistory("EURUSD");
fcsapi.Forex.GetHistory("EURUSD", "1D", 500);
fcsapi.Forex.GetHistory("EURUSD", "1h", 300, "2025-01-01", "2025-01-31");
fcsapi.Forex.GetHistory("EURUSD", "1D", 300, null, null, 2);  // Page 2

// ==================== Profile ====================
fcsapi.Forex.GetProfile("EUR");
fcsapi.Forex.GetProfile("EUR,USD,GBP");

// ==================== Exchanges ====================
fcsapi.Forex.GetExchanges();

// ==================== Technical Analysis ====================
fcsapi.Forex.GetMovingAverages("EURUSD", "1D");  // EMA & SMA
fcsapi.Forex.GetIndicators("EURUSD", "1D");      // RSI, MACD, Stochastic, etc.
fcsapi.Forex.GetPivotPoints("EURUSD", "1D");     // Pivot Points

// ==================== Performance ====================
fcsapi.Forex.GetPerformance("EURUSD");           // Highs, lows, volatility

// ==================== Economy Calendar ====================
fcsapi.Forex.GetEconomyCalendar();
fcsapi.Forex.GetEconomyCalendar("US", "2025-01-01", "2025-01-31");

// ==================== Top Movers ====================
fcsapi.Forex.GetTopGainers();
fcsapi.Forex.GetTopLosers();
fcsapi.Forex.GetMostActive();

// ==================== Search ====================
fcsapi.Forex.Search("EUR");

// ==================== Advanced Query ====================
fcsapi.Forex.Advanced(new Dictionary<string, object> {
    {"type", "forex"},
    {"period", "1D"},
    {"sort_by", "active.chp_desc"},
    {"per_page", 50},
    {"merge", "latest,profile,tech"}
});
```

### Crypto API

```csharp
// ==================== Symbol List ====================
fcsapi.Crypto.GetSymbolsList();                   // All crypto
fcsapi.Crypto.GetSymbolsList("crypto", "binance"); // Binance only
fcsapi.Crypto.GetCoinsList();                     // Coins with market cap

// ==================== Latest Prices ====================
fcsapi.Crypto.GetLatestPrice("BTCUSDT");
fcsapi.Crypto.GetLatestPrice("BINANCE:BTCUSDT,BINANCE:ETHUSDT");
fcsapi.Crypto.GetAllPrices("binance");

// ==================== Coin Data (Market Cap, Rank, Supply) ====================
fcsapi.Crypto.GetCoinData();                      // Top coins with full data
fcsapi.Crypto.GetTopByMarketCap(100);            // Top 100 by market cap
fcsapi.Crypto.GetTopByRank(50);                  // Top 50 by rank

// ==================== Crypto Converter ====================
fcsapi.Crypto.Convert("BTC", "USD", 1);          // 1 BTC to USD
fcsapi.Crypto.Convert("ETH", "BTC", 10);         // 10 ETH to BTC

// ==================== Base Currency ====================
fcsapi.Crypto.GetBasePrices("BTC");              // BTC to all
fcsapi.Crypto.GetBasePrices("USD");              // USD to all cryptos

// ==================== Cross Rates ====================
fcsapi.Crypto.GetCrossRates("USD", "crypto", "1D");

// ==================== Historical Data ====================
fcsapi.Crypto.GetHistory("BINANCE:BTCUSDT");
fcsapi.Crypto.GetHistory("BTCUSDT", "1D", 500);

// ==================== Profile ====================
fcsapi.Crypto.GetProfile("BTC");
fcsapi.Crypto.GetProfile("BTC,ETH,SOL");

// ==================== Exchanges ====================
fcsapi.Crypto.GetExchanges();

// ==================== Technical Analysis ====================
fcsapi.Crypto.GetMovingAverages("BINANCE:BTCUSDT", "1D");
fcsapi.Crypto.GetIndicators("BINANCE:BTCUSDT", "1D");
fcsapi.Crypto.GetPivotPoints("BINANCE:BTCUSDT", "1D");

// ==================== Performance ====================
fcsapi.Crypto.GetPerformance("BINANCE:BTCUSDT");

// ==================== Top Movers ====================
fcsapi.Crypto.GetTopGainers();
fcsapi.Crypto.GetTopGainers("binance", 50);
fcsapi.Crypto.GetTopLosers();
fcsapi.Crypto.GetHighestVolume();

// ==================== Search ====================
fcsapi.Crypto.Search("bitcoin");
```

### Stock API

```csharp
// ==================== Symbol List ====================
fcsapi.Stock.GetSymbolsList();                    // All stocks
fcsapi.Stock.GetSymbolsList("NASDAQ");           // NASDAQ only
fcsapi.Stock.GetSymbolsList(null, "united-states"); // US stocks
fcsapi.Stock.GetSymbolsList(null, null, "technology"); // Tech sector

// ==================== Indices ====================
fcsapi.Stock.GetIndicesList("united-states");    // US indices
fcsapi.Stock.GetIndicesLatest();                 // All indices prices
fcsapi.Stock.GetIndicesLatest("NASDAQ:NDX,SP:SPX"); // Specific indices

// ==================== Latest Prices ====================
fcsapi.Stock.GetLatestPrice("AAPL");
fcsapi.Stock.GetLatestPrice("NASDAQ:AAPL,NASDAQ:GOOGL");
fcsapi.Stock.GetAllPrices("NASDAQ");
fcsapi.Stock.GetLatestByCountry("united-states", "technology");
fcsapi.Stock.GetLatestByIndices("NASDAQ:NDX");  // Stocks in NASDAQ 100

// ==================== Historical Data ====================
fcsapi.Stock.GetHistory("NASDAQ:AAPL");
fcsapi.Stock.GetHistory("AAPL", "1D", 500);

// ==================== Profile ====================
fcsapi.Stock.GetProfile("AAPL");
fcsapi.Stock.GetProfile("NASDAQ:AAPL,NASDAQ:GOOGL");

// ==================== Exchanges ====================
fcsapi.Stock.GetExchanges();

// ==================== Financial Data ====================
fcsapi.Stock.GetEarnings("NASDAQ:AAPL");         // EPS, Revenue
fcsapi.Stock.GetEarnings("NASDAQ:AAPL", "annual"); // Annual only
fcsapi.Stock.GetRevenue("NASDAQ:AAPL");          // Revenue segments
fcsapi.Stock.GetBalanceSheet("NASDAQ:AAPL", "annual");
fcsapi.Stock.GetIncomeStatements("NASDAQ:AAPL", "annual");
fcsapi.Stock.GetCashFlow("NASDAQ:AAPL", "annual");
fcsapi.Stock.GetDividends("NASDAQ:AAPL");        // Dividend history
fcsapi.Stock.GetStatistics("NASDAQ:AAPL");
fcsapi.Stock.GetForecast("NASDAQ:AAPL");
fcsapi.Stock.GetStockData("NASDAQ:AAPL", "profile,earnings,dividends");

// ==================== Technical Analysis ====================
fcsapi.Stock.GetMovingAverages("NASDAQ:AAPL", "1D");
fcsapi.Stock.GetIndicators("NASDAQ:AAPL", "1D");
fcsapi.Stock.GetPivotPoints("NASDAQ:AAPL", "1D");

// ==================== Performance ====================
fcsapi.Stock.GetPerformance("NASDAQ:AAPL");

// ==================== Top Movers ====================
fcsapi.Stock.GetTopGainers();
fcsapi.Stock.GetTopGainers("NASDAQ", 50);
fcsapi.Stock.GetTopLosers();
fcsapi.Stock.GetMostActive();

// ==================== Search & Filter ====================
fcsapi.Stock.Search("Apple");
fcsapi.Stock.GetBySector("technology");
fcsapi.Stock.GetByCountry("united-states");

// ==================== Advanced Query ====================
fcsapi.Stock.Advanced(new Dictionary<string, object> {
    {"exchange", "NASDAQ"},
    {"sector", "technology"},
    {"period", "1D"},
    {"sort_by", "active.chp_desc"},
    {"per_page", 50},
    {"merge", "latest,profile"}
});
```

## Response Handling

```csharp
var response = fcsapi.Forex.GetLatestPrice("EURUSD");

// Check if successful
if (fcsapi.IsSuccess())
{
    var data = response["response"];
    Console.WriteLine(data);
}
else
{
    Console.WriteLine($"Error: {fcsapi.GetError()}");
}

// Get last response info
var lastResponse = fcsapi.GetLastResponse();

// Get response data only
var data = fcsapi.GetResponseData();
```

## Time Periods

Available timeframes for price data:

| Period | Description |
|--------|-------------|
| `1` or `1m` | 1 minute |
| `5` or `5m` | 5 minutes |
| `15` or `15m` | 15 minutes |
| `30` or `30m` | 30 minutes |
| `1h` or `60` | 1 hour |
| `4h` or `240` | 4 hours |
| `1D` | 1 day |
| `1W` | 1 week |
| `1M` | 1 month |

## Examples

### Forex Example
```csharp
using FcsApi;

var fcsapi = new FcsApi.FcsApi();

// Get EUR/USD latest price
var response = fcsapi.Forex.GetLatestPrice("EURUSD");
if (fcsapi.IsSuccess())
{
    var items = response["response"] as List<object>;
    foreach (var item in items)
    {
        var dict = item as Dictionary<string, object>;
        Console.WriteLine($"Symbol: {dict["ticker"]}");
        var active = dict["active"] as Dictionary<string, object>;
        Console.WriteLine($"Price: {active["c"]}");
        Console.WriteLine($"Change: {active["chp"]}%");
    }
}

// Convert 1000 EUR to USD
var conversion = fcsapi.Forex.Convert("EUR", "USD", 1000);
if (fcsapi.IsSuccess())
{
    var resp = conversion["response"] as Dictionary<string, object>;
    Console.WriteLine($"1000 EUR = {resp["total"]} USD");
}
```

### Crypto Example
```csharp
using FcsApi;

var fcsapi = new FcsApi.FcsApi();

// Get Bitcoin price from Binance
var response = fcsapi.Crypto.GetLatestPrice("BINANCE:BTCUSDT");
if (fcsapi.IsSuccess())
{
    var items = response["response"] as List<object>;
    var btc = items[0] as Dictionary<string, object>;
    var active = btc["active"] as Dictionary<string, object>;
    Console.WriteLine($"Bitcoin: ${active["c"]:N2}");
}

// Get top 100 coins by market cap
var coins = fcsapi.Crypto.GetTopByMarketCap(100);
if (fcsapi.IsSuccess())
{
    var resp = coins["response"] as Dictionary<string, object>;
    var data = resp["data"] as List<object>;
    foreach (var coin in data)
    {
        var dict = coin as Dictionary<string, object>;
        Console.WriteLine($"{dict["ticker"]}: Rank #{dict["rank"]}");
    }
}
```

### Stock Example
```csharp
using FcsApi;

var fcsapi = new FcsApi.FcsApi();

// Get Apple stock price
var response = fcsapi.Stock.GetLatestPrice("NASDAQ:AAPL");
if (fcsapi.IsSuccess())
{
    var items = response["response"] as List<object>;
    var aapl = items[0] as Dictionary<string, object>;
    var active = aapl["active"] as Dictionary<string, object>;
    Console.WriteLine($"Apple: ${active["c"]}");
}

// Get Apple earnings data
var earnings = fcsapi.Stock.GetEarnings("NASDAQ:AAPL");
if (fcsapi.IsSuccess())
{
    Console.WriteLine("EPS Data Available");
}

// Get US market indices
var indices = fcsapi.Stock.GetIndicesLatest(null, "united-states");
if (fcsapi.IsSuccess())
{
    var items = indices["response"] as List<object>;
    foreach (var index in items)
    {
        var dict = index as Dictionary<string, object>;
        var active = dict["active"] as Dictionary<string, object>;
        Console.WriteLine($"{dict["ticker"]}: {active["c"]}");
    }
}
```

## Get API Key

1. Visit [FCS API](https://fcsapi.com)
2. Sign up for a free account
3. Get your API key from the dashboard

## Documentation

For complete API documentation, visit:
- [Forex API Documentation](https://fcsapi.com/document/forex-api)
- [Crypto API Documentation](https://fcsapi.com/document/crypto-api)
- [Stock API Documentation](https://fcsapi.com/document/stock-api)

## Support

- Email: support@fcsapi.com
- Website: [fcsapi.com](https://fcsapi.com)

## License

MIT License - see [LICENSE](LICENSE) file for details.
