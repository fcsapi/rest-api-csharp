/// <summary>
/// FCS API - Crypto Module
///
/// @package FcsApi
/// @author FCS API <support@fcsapi.com>
/// </summary>

using System.Collections.Generic;

namespace FcsApi
{
    /// <summary>
    /// Crypto API Module
    /// </summary>
    public class FcsCrypto
    {
        private readonly FcsApi _api;
        private const string BASE = "crypto/";

        /// <summary>
        /// Initialize Crypto module
        /// </summary>
        /// <param name="api">FcsApi instance</param>
        public FcsCrypto(FcsApi api)
        {
            _api = api;
        }

        // ==================== Symbol List ====================

        /// <summary>
        /// Get list of all crypto symbols
        /// </summary>
        /// <param name="type">Filter: crypto, coin, futures, dex, dominance</param>
        /// <param name="subType">Filter: spot, swap, index</param>
        /// <param name="exchange">Filter by exchange: BINANCE, COINBASE</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetSymbolsList(string type = "crypto", string subType = null, string exchange = null)
        {
            var parameters = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(type)) parameters["type"] = type;
            if (!string.IsNullOrEmpty(subType)) parameters["sub_type"] = subType;
            if (!string.IsNullOrEmpty(exchange)) parameters["exchange"] = exchange;

            return _api.Request(BASE + "list", parameters);
        }

        /// <summary>
        /// Get list of all coins (with market cap, rank, supply data)
        /// </summary>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetCoinsList()
        {
            return GetSymbolsList("coin");
        }

        // ==================== Latest Prices ====================

        /// <summary>
        /// Get latest prices
        /// </summary>
        /// <param name="symbol">Symbol(s): BTCUSDT,ETHUSDT or BINANCE:BTCUSDT</param>
        /// <param name="period">Time period: 1m,5m,15m,30m,1h,4h,1D,1W,1M</param>
        /// <param name="type">crypto or coin</param>
        /// <param name="exchange">Exchange name (BINANCE, COINBASE)</param>
        /// <param name="getProfile">Include profile info</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetLatestPrice(string symbol, string period = "1D", string type = null,
            string exchange = null, bool getProfile = false)
        {
            var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "period", period }
            };
            if (!string.IsNullOrEmpty(type)) parameters["type"] = type;
            if (!string.IsNullOrEmpty(exchange)) parameters["exchange"] = exchange;
            if (getProfile) parameters["get_profile"] = 1;

            return _api.Request(BASE + "latest", parameters);
        }

        /// <summary>
        /// Get all latest prices by exchange
        /// </summary>
        /// <param name="exchange">Exchange: BINANCE, COINBASE, KRAKEN</param>
        /// <param name="period">Time period</param>
        /// <param name="type">crypto or coin</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetAllPrices(string exchange, string period = "1D", string type = null)
        {
            var parameters = new Dictionary<string, object>
            {
                { "exchange", exchange },
                { "period", period }
            };
            if (!string.IsNullOrEmpty(type)) parameters["type"] = type;

            return _api.Request(BASE + "latest", parameters);
        }

        // ==================== Coin Data (Rank, Market Cap, Supply) ====================

        /// <summary>
        /// Get coin data with rank, market cap, supply, performance
        /// Note: Only works with type=coin (BTCUSD, ETHUSD, etc.)
        /// </summary>
        /// <param name="symbol">Coin symbol: BTCUSD, ETHUSD (optional)</param>
        /// <param name="limit">Number of results</param>
        /// <param name="sortBy">Sort by: rank_asc, market_cap_desc, circulating_supply_desc</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetCoinData(string symbol = null, int limit = 100, string sortBy = "perf.rank_asc")
        {
            var parameters = new Dictionary<string, object>
            {
                { "type", "coin" },
                { "sort_by", sortBy },
                { "per_page", limit },
                { "merge", "latest,perf" }
            };
            if (!string.IsNullOrEmpty(symbol)) parameters["symbol"] = symbol;

            return _api.Request(BASE + "advance", parameters);
        }

        /// <summary>
        /// Get top coins by market cap
        /// </summary>
        /// <param name="limit">Number of results</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetTopByMarketCap(int limit = 100)
        {
            return GetCoinData(null, limit, "perf.market_cap_desc");
        }

        /// <summary>
        /// Get top coins by rank
        /// </summary>
        /// <param name="limit">Number of results</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetTopByRank(int limit = 100)
        {
            return GetCoinData(null, limit, "perf.rank_asc");
        }

        // ==================== Crypto Converter ====================

        /// <summary>
        /// Crypto converter (crypto to fiat or crypto to crypto)
        /// </summary>
        /// <param name="pair1">From: BTC, ETH</param>
        /// <param name="pair2">To: USD, EUR, BTC</param>
        /// <param name="amount">Amount to convert</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> Convert(string pair1, string pair2, double amount = 1)
        {
            return _api.Request(BASE + "converter", new Dictionary<string, object>
            {
                { "pair1", pair1 },
                { "pair2", pair2 },
                { "amount", amount }
            });
        }

        // ==================== Base Currency ====================

        /// <summary>
        /// Get base currency prices (USD to all cryptos, BTC to all)
        /// Symbol accepts only single token: BTC, ETH, USD (not BTCUSDT)
        /// </summary>
        /// <param name="symbol">Single currency: BTC, ETH, USD</param>
        /// <param name="exchange">Exchange filter</param>
        /// <param name="fallback">If pair not found, fetch from other exchanges</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetBasePrices(string symbol, string exchange = null, bool fallback = false)
        {
            var parameters = new Dictionary<string, object> { { "symbol", symbol } };
            if (!string.IsNullOrEmpty(exchange)) parameters["exchange"] = exchange;
            if (fallback) parameters["fallback"] = 1;

            return _api.Request(BASE + "base_latest", parameters);
        }

        // ==================== Cross Currency ====================

        /// <summary>
        /// Get cross currency rates with OHLC data
        /// Returns all pairs of base currency (USD -> USDBTC, USDETH, etc.)
        /// </summary>
        /// <param name="symbol">Single currency: USD, BTC, ETH</param>
        /// <param name="exchange">Exchange filter</param>
        /// <param name="type">crypto or forex</param>
        /// <param name="period">Time period</param>
        /// <param name="crossrates">Return pairwise cross rates between multiple symbols</param>
        /// <param name="fallback">If not found, fetch from other exchanges</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetCrossRates(string symbol, string exchange = null, string type = "crypto",
            string period = "1D", bool crossrates = false, bool fallback = false)
        {
            var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "type", type },
                { "period", period }
            };
            if (!string.IsNullOrEmpty(exchange)) parameters["exchange"] = exchange;
            if (crossrates) parameters["crossrates"] = 1;
            if (fallback) parameters["fallback"] = 1;

            return _api.Request(BASE + "cross", parameters);
        }

        // ==================== Historical Data ====================

        /// <summary>
        /// Get historical prices (OHLCV candles)
        /// </summary>
        /// <param name="symbol">Single symbol: BINANCE:BTCUSDT or BTCUSDT</param>
        /// <param name="period">Time period: 1m,5m,15m,1h,1D</param>
        /// <param name="length">Number of candles (max 10000)</param>
        /// <param name="fromDate">Start date (YYYY-MM-DD)</param>
        /// <param name="toDate">End date (YYYY-MM-DD)</param>
        /// <param name="page">Page number for pagination</param>
        /// <param name="isChart">Return chart-friendly format [timestamp,o,h,l,c,v]</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetHistory(string symbol, string period = "1D", int length = 300,
            string fromDate = null, string toDate = null, int page = 1, bool isChart = false)
        {
            var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "period", period },
                { "length", length },
                { "page", page }
            };
            if (!string.IsNullOrEmpty(fromDate)) parameters["from"] = fromDate;
            if (!string.IsNullOrEmpty(toDate)) parameters["to"] = toDate;
            if (isChart) parameters["is_chart"] = 1;

            return _api.Request(BASE + "history", parameters);
        }

        // ==================== Profile ====================

        /// <summary>
        /// Get coin profile details (name, website, social links, etc.)
        /// </summary>
        /// <param name="symbol">Coin symbol: BTC,ETH,SOL (not pairs)</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetProfile(string symbol)
        {
            return _api.Request(BASE + "profile", new Dictionary<string, object> { { "symbol", symbol } });
        }

        // ==================== Exchanges ====================

        /// <summary>
        /// Get available exchanges
        /// </summary>
        /// <param name="type">crypto, coin, futures, dex</param>
        /// <param name="subType">spot, swap</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetExchanges(string type = null, string subType = null)
        {
            var parameters = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(type)) parameters["type"] = type;
            if (!string.IsNullOrEmpty(subType)) parameters["sub_type"] = subType;

            return _api.Request(BASE + "exchanges", parameters);
        }

        // ==================== Advanced Query ====================

        /// <summary>
        /// Advanced query with filters, sorting, pagination, merging
        /// </summary>
        /// <param name="parameters">Query parameters</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> Advanced(Dictionary<string, object> parameters)
        {
            return _api.Request(BASE + "advance", parameters);
        }

        // ==================== Technical Analysis ====================

        /// <summary>
        /// Get Moving Averages (EMA & SMA)
        /// </summary>
        /// <param name="symbol">Symbol(s): BTCUSDT or BINANCE:BTCUSDT</param>
        /// <param name="period">Time period</param>
        /// <param name="exchange">Exchange filter</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetMovingAverages(string symbol, string period = "1D", string exchange = null)
        {
            var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "period", period }
            };
            if (!string.IsNullOrEmpty(exchange)) parameters["exchange"] = exchange;

            return _api.Request(BASE + "ma_avg", parameters);
        }

        /// <summary>
        /// Get Technical Indicators (RSI, MACD, Stochastic, ADX, ATR, etc.)
        /// </summary>
        /// <param name="symbol">Symbol(s): BTCUSDT</param>
        /// <param name="period">Time period</param>
        /// <param name="exchange">Exchange filter</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetIndicators(string symbol, string period = "1D", string exchange = null)
        {
            var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "period", period }
            };
            if (!string.IsNullOrEmpty(exchange)) parameters["exchange"] = exchange;

            return _api.Request(BASE + "indicators", parameters);
        }

        /// <summary>
        /// Get Pivot Points (Classic, Fibonacci, Camarilla, Woodie, Demark)
        /// </summary>
        /// <param name="symbol">Symbol(s): BTCUSDT</param>
        /// <param name="period">Time period</param>
        /// <param name="exchange">Exchange filter</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetPivotPoints(string symbol, string period = "1D", string exchange = null)
        {
            var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "period", period }
            };
            if (!string.IsNullOrEmpty(exchange)) parameters["exchange"] = exchange;

            return _api.Request(BASE + "pivot_points", parameters);
        }

        // ==================== Performance ====================

        /// <summary>
        /// Get Performance Data (historical highs/lows, percentage changes, volatility)
        /// </summary>
        /// <param name="symbol">Symbol(s): BTCUSDT</param>
        /// <param name="exchange">Exchange filter</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetPerformance(string symbol, string exchange = null)
        {
            var parameters = new Dictionary<string, object> { { "symbol", symbol } };
            if (!string.IsNullOrEmpty(exchange)) parameters["exchange"] = exchange;

            return _api.Request(BASE + "performance", parameters);
        }

        // ==================== Top Movers ====================

        /// <summary>
        /// Get top gainers
        /// </summary>
        /// <param name="exchange">Exchange filter: BINANCE, COINBASE</param>
        /// <param name="limit">Number of results</param>
        /// <param name="period">Time period</param>
        /// <param name="type">crypto or coin</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetTopGainers(string exchange = null, int limit = 20, string period = "1D", string type = "crypto")
        {
            return GetSortedData("active.chp", "desc", limit, type, exchange, period);
        }

        /// <summary>
        /// Get top losers
        /// </summary>
        /// <param name="exchange">Exchange filter</param>
        /// <param name="limit">Number of results</param>
        /// <param name="period">Time period</param>
        /// <param name="type">crypto or coin</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetTopLosers(string exchange = null, int limit = 20, string period = "1D", string type = "crypto")
        {
            return GetSortedData("active.chp", "asc", limit, type, exchange, period);
        }

        /// <summary>
        /// Get highest volume coins
        /// </summary>
        /// <param name="exchange">Exchange filter</param>
        /// <param name="limit">Number of results</param>
        /// <param name="period">Time period</param>
        /// <param name="type">crypto or coin</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetHighestVolume(string exchange = null, int limit = 20, string period = "1D", string type = "crypto")
        {
            return GetSortedData("active.v", "desc", limit, type, exchange, period);
        }

        // ==================== Custom Sorting ====================

        /// <summary>
        /// Get data with custom sorting
        /// User can specify any column and sort direction
        /// </summary>
        /// <param name="sortColumn">Column to sort: active.c, active.chp, active.v, active.h, active.l, rank, market_cap</param>
        /// <param name="sortDirection">Sort direction: asc or desc</param>
        /// <param name="limit">Number of results</param>
        /// <param name="type">crypto, coin, futures, dex</param>
        /// <param name="exchange">Exchange filter: BINANCE, COINBASE</param>
        /// <param name="period">Time period</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetSortedData(string sortColumn, string sortDirection = "desc", int limit = 20,
            string type = "crypto", string exchange = null, string period = "1D")
        {
            var parameters = new Dictionary<string, object>
            {
                { "period", period },
                { "sort_by", $"{sortColumn}_{sortDirection}" },
                { "per_page", limit },
                { "merge", "latest" }
            };
            if (!string.IsNullOrEmpty(type)) parameters["type"] = type;
            if (!string.IsNullOrEmpty(exchange)) parameters["exchange"] = exchange;

            return _api.Request(BASE + "advance", parameters);
        }

        // ==================== Search ====================

        /// <summary>
        /// Search coins/tokens
        /// </summary>
        /// <param name="query">Search term (BTC, ethereum, doge)</param>
        /// <param name="type">crypto, coin, futures, dex</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> Search(string query, string type = null)
        {
            var parameters = new Dictionary<string, object> { { "search", query } };
            if (!string.IsNullOrEmpty(type)) parameters["type"] = type;

            return _api.Request(BASE + "list", parameters);
        }

        // ==================== Multiple/Parallel Requests ====================

        /// <summary>
        /// Execute multiple API requests in parallel
        /// </summary>
        /// <param name="urls">Array of API endpoints</param>
        /// <param name="baseUrl">Common URL base</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> MultiUrl(string[] urls, string baseUrl = null)
        {
            var parameters = new Dictionary<string, object> { { "url", string.Join(",", urls) } };
            if (!string.IsNullOrEmpty(baseUrl)) parameters["base"] = baseUrl;

            return _api.Request(BASE + "multi_url", parameters);
        }
    }
}
