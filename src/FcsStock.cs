/// <summary>
/// FCS API - Stock Module
///
/// @package FcsApi
/// @author FCS API <support@fcsapi.com>
/// </summary>

using System.Collections.Generic;

namespace FcsApi
{
    /// <summary>
    /// Stock API Module
    /// </summary>
    public class FcsStock
    {
        private readonly FcsApi _api;
        private const string BASE = "stock/";

        /// <summary>
        /// Initialize Stock module
        /// </summary>
        /// <param name="api">FcsApi instance</param>
        public FcsStock(FcsApi api)
        {
            _api = api;
        }

        // ==================== Symbol/Stock List ====================

        /// <summary>
        /// Get list of all stock symbols
        /// </summary>
        /// <param name="exchange">Filter by exchange: NASDAQ, NYSE, BSE</param>
        /// <param name="country">Filter by country: united-states, japan, india</param>
        /// <param name="sector">Filter by sector: technology, finance, energy</param>
        /// <param name="indices">Filter by indices: DJ:DJI, NASDAQ:IXIC</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetSymbolsList(string exchange = null, string country = null,
            string sector = null, string indices = null)
        {
            var parameters = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(exchange)) parameters["exchange"] = exchange;
            if (!string.IsNullOrEmpty(country)) parameters["country"] = country;
            if (!string.IsNullOrEmpty(sector)) parameters["sector"] = sector;
            if (!string.IsNullOrEmpty(indices)) parameters["indices"] = indices;

            return _api.Request(BASE + "list", parameters);
        }

        // ==================== Indices ====================

        /// <summary>
        /// Get list of market indices by country
        /// </summary>
        /// <param name="country">Country name: united-states, japan</param>
        /// <param name="exchange">Exchange filter: nasdaq, nyse</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetIndicesList(string country = null, string exchange = null)
        {
            var parameters = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(country)) parameters["country"] = country;
            if (!string.IsNullOrEmpty(exchange)) parameters["exchange"] = exchange;

            return _api.Request(BASE + "indices", parameters);
        }

        /// <summary>
        /// Get latest index prices
        /// </summary>
        /// <param name="symbol">Index symbol(s): NASDAQ:NDX, SP:SPX</param>
        /// <param name="country">Country filter</param>
        /// <param name="exchange">Exchange filter</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetIndicesLatest(string symbol = null, string country = null, string exchange = null)
        {
            var parameters = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(symbol)) parameters["symbol"] = symbol;
            if (!string.IsNullOrEmpty(country)) parameters["country"] = country;
            if (!string.IsNullOrEmpty(exchange)) parameters["exchange"] = exchange;

            return _api.Request(BASE + "indices_latest", parameters);
        }

        // ==================== Latest Prices ====================

        /// <summary>
        /// Get latest stock prices
        /// </summary>
        /// <param name="symbol">Symbol(s): AAPL,GOOGL or NASDAQ:AAPL</param>
        /// <param name="period">Time period: 1m,5m,15m,30m,1h,4h,1D,1W,1M</param>
        /// <param name="exchange">Exchange name</param>
        /// <param name="getProfile">Include profile info</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetLatestPrice(string symbol, string period = "1D", string exchange = null, bool getProfile = false)
        {
            var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "period", period },
                { "get_profile", getProfile ? 1 : 0 }
            };
            if (!string.IsNullOrEmpty(exchange)) parameters["exchange"] = exchange;

            return _api.Request(BASE + "latest", parameters);
        }

        /// <summary>
        /// Get all latest prices by exchange
        /// </summary>
        /// <param name="exchange">Exchange: NASDAQ, NYSE, LSE</param>
        /// <param name="period">Time period</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetAllPrices(string exchange, string period = "1D")
        {
            return _api.Request(BASE + "latest", new Dictionary<string, object>
            {
                { "exchange", exchange },
                { "period", period }
            });
        }

        /// <summary>
        /// Get latest prices by country and sector
        /// </summary>
        /// <param name="country">Country: united-states, japan</param>
        /// <param name="sector">Sector: technology, finance</param>
        /// <param name="period">Time period</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetLatestByCountry(string country, string sector = null, string period = "1D")
        {
            var parameters = new Dictionary<string, object>
            {
                { "country", country },
                { "period", period }
            };
            if (!string.IsNullOrEmpty(sector)) parameters["sector"] = sector;

            return _api.Request(BASE + "latest", parameters);
        }

        /// <summary>
        /// Get latest prices by indices
        /// </summary>
        /// <param name="indices">Indices IDs: NASDAQ:NDX, SP:SPX</param>
        /// <param name="period">Time period</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetLatestByIndices(string indices, string period = "1D")
        {
            return _api.Request(BASE + "latest", new Dictionary<string, object>
            {
                { "indices", indices },
                { "period", period }
            });
        }

        // ==================== Historical Data ====================

        /// <summary>
        /// Get historical prices (works for stocks and indices)
        /// </summary>
        /// <param name="symbol">Single symbol: AAPL or NASDAQ:AAPL</param>
        /// <param name="period">Time period</param>
        /// <param name="length">Number of candles (max 10000)</param>
        /// <param name="fromDate">Start date</param>
        /// <param name="toDate">End date</param>
        /// <param name="page">Page number for pagination</param>
        /// <param name="isChart">Return chart-friendly format</param>
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
        /// Get stock profile/company details
        /// </summary>
        /// <param name="symbol">Stock symbol: AAPL,GOOGL or NASDAQ:AAPL</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetProfile(string symbol)
        {
            return _api.Request(BASE + "profile", new Dictionary<string, object> { { "symbol", symbol } });
        }

        // ==================== Exchanges ====================

        /// <summary>
        /// Get available exchanges
        /// </summary>
        /// <param name="type">Filter: stock, all_stock</param>
        /// <param name="subType">Filter: equity, etf</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetExchanges(string type = null, string subType = null)
        {
            var parameters = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(type)) parameters["type"] = type;
            if (!string.IsNullOrEmpty(subType)) parameters["sub_type"] = subType;

            return _api.Request(BASE + "exchanges", parameters);
        }

        // ==================== Financial Data ====================

        /// <summary>
        /// Get earnings data (EPS, Revenue)
        /// </summary>
        /// <param name="symbol">Stock symbol: NASDAQ:AAPL (can be multiple comma-separated)</param>
        /// <param name="duration">Filter: annual, interim, both</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetEarnings(string symbol, string duration = "both")
        {
            return _api.Request(BASE + "earnings", new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "duration", duration }
            });
        }

        /// <summary>
        /// Get revenue segmentation data (by business and region)
        /// </summary>
        /// <param name="symbol">Stock symbol: NASDAQ:AAPL (can be multiple comma-separated)</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetRevenue(string symbol)
        {
            return _api.Request(BASE + "revenue", new Dictionary<string, object> { { "symbol", symbol } });
        }

        /// <summary>
        /// Get dividends data (payment dates, amounts, yield)
        /// </summary>
        /// <param name="symbol">Stock symbol: NASDAQ:AAPL</param>
        /// <param name="format">Response format: plain (default), inherit (nested array)</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetDividends(string symbol, string format = "plain")
        {
            return _api.Request(BASE + "dividend", new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "format", format }
            });
        }

        /// <summary>
        /// Get balance sheet data
        /// </summary>
        /// <param name="symbol">Stock symbol: NASDAQ:AAPL</param>
        /// <param name="duration">annual, interim</param>
        /// <param name="format">Response format: plain, inherit</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetBalanceSheet(string symbol, string duration = "annual", string format = "plain")
        {
            return _api.Request(BASE + "balance_sheet", new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "duration", duration },
                { "format", format }
            });
        }

        /// <summary>
        /// Get income statement data
        /// </summary>
        /// <param name="symbol">Stock symbol: NASDAQ:AAPL</param>
        /// <param name="duration">annual, interim</param>
        /// <param name="format">Response format: plain, inherit</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetIncomeStatements(string symbol, string duration = "annual", string format = "plain")
        {
            return _api.Request(BASE + "income_statements", new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "duration", duration },
                { "format", format }
            });
        }

        /// <summary>
        /// Get cash flow statement data
        /// </summary>
        /// <param name="symbol">Stock symbol: NASDAQ:AAPL</param>
        /// <param name="duration">annual, interim</param>
        /// <param name="format">Response format: plain, inherit</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetCashFlow(string symbol, string duration = "annual", string format = "plain")
        {
            return _api.Request(BASE + "cash_flow", new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "duration", duration },
                { "format", format }
            });
        }

        /// <summary>
        /// Get stock statistics and financial ratios
        /// </summary>
        /// <param name="symbol">Stock symbol: NASDAQ:AAPL</param>
        /// <param name="duration">annual, interim</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetStatistics(string symbol, string duration = "annual")
        {
            return _api.Request(BASE + "statistics", new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "duration", duration }
            });
        }

        /// <summary>
        /// Get price target forecast from analysts
        /// </summary>
        /// <param name="symbol">Stock symbol: NASDAQ:AAPL</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetForecast(string symbol)
        {
            return _api.Request(BASE + "forecast", new Dictionary<string, object> { { "symbol", symbol } });
        }

        /// <summary>
        /// Get combined financial data (multiple endpoints in one call)
        /// </summary>
        /// <param name="symbol">Stock symbol: NASDAQ:AAPL</param>
        /// <param name="dataColumn">Comma-separated: earnings,revenue,profile,dividends,balance_sheet,income_statements,statistics,cash_flow</param>
        /// <param name="duration">annual, interim</param>
        /// <param name="format">Response format: plain, inherit</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetStockData(string symbol, string dataColumn = "profile,earnings,dividends",
            string duration = "annual", string format = "plain")
        {
            return _api.Request(BASE + "stock_data", new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "data_column", dataColumn },
                { "duration", duration },
                { "format", format }
            });
        }

        // ==================== Technical Analysis ====================

        /// <summary>
        /// Get Moving Averages (EMA & SMA)
        /// </summary>
        /// <param name="symbol">Symbol: NASDAQ:AAPL</param>
        /// <param name="period">Time period</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetMovingAverages(string symbol, string period = "1D")
        {
            return _api.Request(BASE + "ma_avg", new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "period", period }
            });
        }

        /// <summary>
        /// Get Technical Indicators (RSI, MACD, Stochastic, ADX, ATR, etc.)
        /// </summary>
        /// <param name="symbol">Symbol: NASDAQ:AAPL</param>
        /// <param name="period">Time period</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetIndicators(string symbol, string period = "1D")
        {
            return _api.Request(BASE + "indicators", new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "period", period }
            });
        }

        /// <summary>
        /// Get Pivot Points
        /// </summary>
        /// <param name="symbol">Symbol: NASDAQ:AAPL</param>
        /// <param name="period">Time period</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetPivotPoints(string symbol, string period = "1D")
        {
            return _api.Request(BASE + "pivot_points", new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "period", period }
            });
        }

        // ==================== Performance ====================

        /// <summary>
        /// Get Performance Data (historical highs/lows, percentage changes, volatility)
        /// </summary>
        /// <param name="symbol">Symbol: NASDAQ:AAPL</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetPerformance(string symbol)
        {
            return _api.Request(BASE + "performance", new Dictionary<string, object> { { "symbol", symbol } });
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

        // ==================== Top Movers ====================

        /// <summary>
        /// Get top gainers
        /// </summary>
        /// <param name="exchange">Exchange filter: NASDAQ, NYSE</param>
        /// <param name="limit">Number of results</param>
        /// <param name="period">Time period</param>
        /// <param name="country">Country filter</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetTopGainers(string exchange = null, int limit = 20, string period = "1D", string country = null)
        {
            return GetSortedData("active.chp", "desc", limit, exchange, country, period);
        }

        /// <summary>
        /// Get top losers
        /// </summary>
        /// <param name="exchange">Exchange filter</param>
        /// <param name="limit">Number of results</param>
        /// <param name="period">Time period</param>
        /// <param name="country">Country filter</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetTopLosers(string exchange = null, int limit = 20, string period = "1D", string country = null)
        {
            return GetSortedData("active.chp", "asc", limit, exchange, country, period);
        }

        /// <summary>
        /// Get most active stocks by volume
        /// </summary>
        /// <param name="exchange">Exchange filter</param>
        /// <param name="limit">Number of results</param>
        /// <param name="period">Time period</param>
        /// <param name="country">Country filter</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetMostActive(string exchange = null, int limit = 20, string period = "1D", string country = null)
        {
            return GetSortedData("active.v", "desc", limit, exchange, country, period);
        }

        // ==================== Custom Sorting ====================

        /// <summary>
        /// Get data with custom sorting
        /// User can specify any column and sort direction
        /// </summary>
        /// <param name="sortColumn">Column to sort: active.c, active.chp, active.v, active.h, active.l</param>
        /// <param name="sortDirection">Sort direction: asc or desc</param>
        /// <param name="limit">Number of results</param>
        /// <param name="exchange">Exchange filter: NASDAQ, NYSE, LSE</param>
        /// <param name="country">Country filter</param>
        /// <param name="period">Time period</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetSortedData(string sortColumn, string sortDirection = "desc", int limit = 20,
            string exchange = null, string country = null, string period = "1D")
        {
            var parameters = new Dictionary<string, object>
            {
                { "period", period },
                { "sort_by", $"{sortColumn}_{sortDirection}" },
                { "per_page", limit },
                { "merge", "latest" }
            };
            if (!string.IsNullOrEmpty(exchange)) parameters["exchange"] = exchange;
            if (!string.IsNullOrEmpty(country)) parameters["country"] = country;

            return Advanced(parameters);
        }

        // ==================== Search ====================

        /// <summary>
        /// Search stocks
        /// </summary>
        /// <param name="query">Search term</param>
        /// <param name="exchange">Exchange filter</param>
        /// <param name="country">Country filter</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> Search(string query, string exchange = null, string country = null)
        {
            var parameters = new Dictionary<string, object> { { "search", query } };
            if (!string.IsNullOrEmpty(exchange)) parameters["exchange"] = exchange;
            if (!string.IsNullOrEmpty(country)) parameters["country"] = country;

            return _api.Request(BASE + "list", parameters);
        }

        // ==================== Filter by Sector/Country ====================

        /// <summary>
        /// Get stocks by sector
        /// </summary>
        /// <param name="sector">Sector: technology, finance, energy, healthcare</param>
        /// <param name="limit">Number of results</param>
        /// <param name="exchange">Exchange filter</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetBySector(string sector, int limit = 100, string exchange = null)
        {
            var parameters = new Dictionary<string, object>
            {
                { "sector", sector },
                { "per_page", limit },
                { "merge", "latest" }
            };
            if (!string.IsNullOrEmpty(exchange)) parameters["exchange"] = exchange;

            return Advanced(parameters);
        }

        /// <summary>
        /// Get stocks by country
        /// </summary>
        /// <param name="country">Country: united-states, japan, india</param>
        /// <param name="limit">Number of results</param>
        /// <param name="exchange">Exchange filter</param>
        /// <returns>API response or null</returns>
        public Dictionary<string, object> GetByCountry(string country, int limit = 100, string exchange = null)
        {
            var parameters = new Dictionary<string, object>
            {
                { "country", country },
                { "per_page", limit },
                { "merge", "latest" }
            };
            if (!string.IsNullOrEmpty(exchange)) parameters["exchange"] = exchange;

            return Advanced(parameters);
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
