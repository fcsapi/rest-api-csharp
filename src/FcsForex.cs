/// <summary>
/// FCS API - Forex Module
///
/// @package FcsApi
/// @author FCS API <support@fcsapi.com>
/// </summary>

using System.Collections.Generic;

namespace FcsApi
{
    /// <summary>
    /// Forex API Module
    /// </summary>
    public class FcsForex
    {
        private readonly FcsApi _api;
        private const string BASE = "forex/";

        /// <summary>
        /// Initialize Forex module
        /// </summary>
        /// <param name="api">FcsApi instance</param>
        public FcsForex(FcsApi api)
        {
            _api = api;
        }

        // ==================== Symbol List ====================

        /// <summary>
        /// Get list of all forex symbols
        /// </summary>
        /// <param name="type">Filter by type: forex, commodity</param>
        /// <param name="subType">Filter: spot, synthetic</param>
        /// <param name="exchange">Filter by exchange: FX, ONA, SFO, FCM</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetSymbolsList(string type = null, string subType = null, string exchange = null)
        {
            var parameters = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(type)) parameters["type"] = type;
            if (!string.IsNullOrEmpty(subType)) parameters["sub_type"] = subType;
            if (!string.IsNullOrEmpty(exchange)) parameters["exchange"] = exchange;

            return _api.Request(BASE + "list", parameters);
        }

        // ==================== Latest Prices ====================

        /// <summary>
        /// Get latest prices for symbols
        /// </summary>
        /// <param name="symbol">Symbol(s) comma-separated: EURUSD,GBPUSD or FX:EURUSD</param>
        /// <param name="period">Time period: 1m,5m,15m,30m,1h,4h,1D,1W,1M</param>
        /// <param name="type">forex or commodity</param>
        /// <param name="exchange">Exchange name: FX, ONA, SFO</param>
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
        /// <param name="exchange">Exchange name: FX, ONA, SFO</param>
        /// <param name="period">Time period</param>
        /// <param name="type">forex or commodity</param>
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

        // ==================== Commodities ====================

        /// <summary>
        /// Get commodity prices (Gold, Silver, Oil, etc.)
        /// </summary>
        /// <param name="symbol">Commodity symbol: XAUUSD, XAGUSD, USOIL, BRENT, NGAS</param>
        /// <param name="period">Time period</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetCommodities(string symbol = null, string period = "1D")
        {
            var parameters = new Dictionary<string, object>
            {
                { "type", "commodity" },
                { "period", period }
            };
            if (!string.IsNullOrEmpty(symbol)) parameters["symbol"] = symbol;

            return _api.Request(BASE + "latest", parameters);
        }

        /// <summary>
        /// Get commodity symbols list
        /// </summary>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetCommoditySymbols()
        {
            return GetSymbolsList("commodity");
        }

        // ==================== Currency Converter ====================

        /// <summary>
        /// Currency converter
        /// </summary>
        /// <param name="pair1">Currency From: EUR, USD</param>
        /// <param name="pair2">Currency To: USD, GBP</param>
        /// <param name="amount">Amount to convert</param>
        /// <param name="type">forex or crypto</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> Convert(string pair1, string pair2, double amount = 1, string type = null)
        {
            var parameters = new Dictionary<string, object>
            {
                { "pair1", pair1 },
                { "pair2", pair2 },
                { "amount", amount }
            };
            if (!string.IsNullOrEmpty(type)) parameters["type"] = type;

            return _api.Request(BASE + "converter", parameters);
        }

        // ==================== Base Currency ====================

        /// <summary>
        /// Get base currency prices (USD to all currencies)
        /// Symbol accepts only single currency: USD, EUR, JPY (not USDJPY)
        /// </summary>
        /// <param name="symbol">Single currency code: USD, EUR, JPY</param>
        /// <param name="type">forex or crypto</param>
        /// <param name="exchange">Exchange filter</param>
        /// <param name="fallback">If not found, fetch from other exchanges</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetBasePrices(string symbol, string type = "forex", string exchange = null, bool fallback = false)
        {
            var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "type", type }
            };
            if (!string.IsNullOrEmpty(exchange)) parameters["exchange"] = exchange;
            if (fallback) parameters["fallback"] = 1;

            return _api.Request(BASE + "base_latest", parameters);
        }

        // ==================== Cross Currency ====================

        /// <summary>
        /// Get cross currency rates with OHLC data
        /// Returns all pairs of base currency (USD -> USDEUR, USDGBP, USDJPY, etc.)
        /// </summary>
        /// <param name="symbol">Single currency: USD, EUR, JPY</param>
        /// <param name="exchange">Exchange filter</param>
        /// <param name="type">forex or crypto</param>
        /// <param name="period">Time period</param>
        /// <param name="crossrates">Return pairwise cross rates between multiple symbols</param>
        /// <param name="fallback">If not found, fetch from other exchanges</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetCrossRates(string symbol, string exchange = null, string type = "forex",
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
        /// <param name="symbol">Single symbol: EURUSD or FX:EURUSD</param>
        /// <param name="period">Time period: 1m,5m,15m,1h,1D</param>
        /// <param name="length">Number of candles (max 10000)</param>
        /// <param name="fromDate">Start date (YYYY-MM-DD or unix)</param>
        /// <param name="toDate">End date (YYYY-MM-DD or unix)</param>
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
        /// Get currency profile details (name, country, bank, etc.)
        /// </summary>
        /// <param name="symbol">Currency codes: EUR,USD,GBP (not pairs)</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetProfile(string symbol)
        {
            return _api.Request(BASE + "profile", new Dictionary<string, object> { { "symbol", symbol } });
        }

        // ==================== Exchanges ====================

        /// <summary>
        /// Get available exchanges/data sources
        /// </summary>
        /// <param name="type">forex or commodity</param>
        /// <param name="subType">spot, synthetic</param>
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
        /// <param name="symbol">Symbol(s): EURUSD or FX:EURUSD</param>
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
        /// <param name="symbol">Symbol(s): EURUSD</param>
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
        /// <param name="symbol">Symbol(s): EURUSD</param>
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
        /// <param name="symbol">Symbol(s): EURUSD</param>
        /// <param name="exchange">Exchange filter</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetPerformance(string symbol, string exchange = null)
        {
            var parameters = new Dictionary<string, object> { { "symbol", symbol } };
            if (!string.IsNullOrEmpty(exchange)) parameters["exchange"] = exchange;

            return _api.Request(BASE + "performance", parameters);
        }

        // ==================== Economy Calendar ====================

        /// <summary>
        /// Get Economic Calendar Events
        /// </summary>
        /// <param name="symbol">Filter by currency: USD, EUR, GBP</param>
        /// <param name="country">Country filter: US, GB, DE, JP</param>
        /// <param name="fromDate">Start date (YYYY-MM-DD)</param>
        /// <param name="toDate">End date (YYYY-MM-DD)</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetEconomyCalendar(string symbol = null, string country = null,
            string fromDate = null, string toDate = null)
        {
            var parameters = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(symbol)) parameters["symbol"] = symbol;
            if (!string.IsNullOrEmpty(country)) parameters["country"] = country;
            if (!string.IsNullOrEmpty(fromDate)) parameters["from"] = fromDate;
            if (!string.IsNullOrEmpty(toDate)) parameters["to"] = toDate;

            return _api.Request(BASE + "economy_cal", parameters);
        }

        // ==================== Top Movers ====================

        /// <summary>
        /// Get top gainers
        /// </summary>
        /// <param name="type">forex or commodity</param>
        /// <param name="limit">Number of results</param>
        /// <param name="period">Time period</param>
        /// <param name="exchange">Exchange filter</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetTopGainers(string type = "forex", int limit = 20, string period = "1D", string exchange = null)
        {
            return GetSortedData("active.chp", "desc", limit, type, exchange, period);
        }

        /// <summary>
        /// Get top losers
        /// </summary>
        /// <param name="type">forex or commodity</param>
        /// <param name="limit">Number of results</param>
        /// <param name="period">Time period</param>
        /// <param name="exchange">Exchange filter</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetTopLosers(string type = "forex", int limit = 20, string period = "1D", string exchange = null)
        {
            return GetSortedData("active.chp", "asc", limit, type, exchange, period);
        }

        /// <summary>
        /// Get most active by volume
        /// </summary>
        /// <param name="type">forex or commodity</param>
        /// <param name="limit">Number of results</param>
        /// <param name="period">Time period</param>
        /// <param name="exchange">Exchange filter</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetMostActive(string type = "forex", int limit = 20, string period = "1D", string exchange = null)
        {
            return GetSortedData("active.v", "desc", limit, type, exchange, period);
        }

        // ==================== Custom Sorting ====================

        /// <summary>
        /// Get data with custom sorting
        /// User can specify any column and sort direction
        /// </summary>
        /// <param name="sortColumn">Column to sort: active.c, active.chp, active.v, active.h, active.l</param>
        /// <param name="sortDirection">Sort direction: asc or desc</param>
        /// <param name="limit">Number of results</param>
        /// <param name="type">forex or commodity</param>
        /// <param name="exchange">Exchange filter: FX, ONA, SFO</param>
        /// <param name="period">Time period</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetSortedData(string sortColumn, string sortDirection = "desc", int limit = 20,
            string type = "forex", string exchange = null, string period = "1D")
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

            return Advanced(parameters);
        }

        // ==================== Search ====================

        /// <summary>
        /// Search symbols
        /// </summary>
        /// <param name="query">Search term (EUR, USD, gold)</param>
        /// <param name="type">forex or commodity</param>
        /// <param name="exchange">Exchange filter</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> Search(string query, string type = null, string exchange = null)
        {
            var parameters = new Dictionary<string, object> { { "search", query } };
            if (!string.IsNullOrEmpty(type)) parameters["type"] = type;
            if (!string.IsNullOrEmpty(exchange)) parameters["exchange"] = exchange;

            return _api.Request(BASE + "search", parameters);
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
