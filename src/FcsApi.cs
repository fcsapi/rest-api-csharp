/// <summary>
/// FCS API - REST API Client
///
/// C# client for Forex, Cryptocurrency, and Stock market data
///
/// @package FcsApi
/// @author FCS API <support@fcsapi.com>
/// @link https://fcsapi.com
/// </summary>

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FcsApi
{
    /// <summary>
    /// FCS API REST Client
    /// Main client class for accessing Forex, Crypto, and Stock market data.
    /// </summary>
    public class FcsApi
    {
        private const string BASE_URL = "https://api-v4.fcsapi.com/";
        private readonly HttpClient _httpClient;
        private Dictionary<string, object> _lastResponse = new Dictionary<string, object>();

        /// <summary>
        /// Configuration instance
        /// </summary>
        public FcsConfig Config { get; private set; }

        // Lazy-loaded modules
        private FcsForex _forex;
        private FcsCrypto _crypto;
        private FcsStock _stock;

        /// <summary>
        /// Get Forex API module (lazy loading)
        /// </summary>
        public FcsForex Forex => _forex ??= new FcsForex(this);

        /// <summary>
        /// Get Crypto API module (lazy loading)
        /// </summary>
        public FcsCrypto Crypto => _crypto ??= new FcsCrypto(this);

        /// <summary>
        /// Get Stock API module (lazy loading)
        /// </summary>
        public FcsStock Stock => _stock ??= new FcsStock(this);

        /// <summary>
        /// Constructor with FcsConfig
        /// </summary>
        /// <param name="config">FcsConfig object or null to use default config</param>
        public FcsApi(FcsConfig config = null)
        {
            Config = config ?? new FcsConfig();
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(Config.Timeout)
            };
        }

        /// <summary>
        /// Constructor with API key string
        /// </summary>
        /// <param name="apiKey">API key string</param>
        public FcsApi(string apiKey) : this(FcsConfig.WithAccessKey(apiKey))
        {
        }

        /// <summary>
        /// Set request timeout
        /// </summary>
        /// <param name="seconds">Timeout in seconds</param>
        /// <returns>Self for method chaining</returns>
        public FcsApi SetTimeout(int seconds)
        {
            Config.Timeout = seconds;
            _httpClient.Timeout = TimeSpan.FromSeconds(seconds);
            return this;
        }

        /// <summary>
        /// Get config
        /// </summary>
        /// <returns>FcsConfig instance</returns>
        public FcsConfig GetConfig()
        {
            return Config;
        }

        /// <summary>
        /// Generate token for frontend use
        /// Only works when AuthMethod is 'token'
        /// </summary>
        /// <returns>Dictionary with _token, _expiry, _public_key</returns>
        public Dictionary<string, object> GenerateToken()
        {
            return Config.GenerateToken();
        }

        /// <summary>
        /// Make API request (POST with form data)
        /// </summary>
        /// <param name="endpoint">API endpoint</param>
        /// <param name="parameters">Request parameters</param>
        /// <returns>Response data or null on error</returns>
        public async Task<Dictionary<string, object>> RequestAsync(string endpoint, Dictionary<string, object> parameters = null)
        {
            parameters ??= new Dictionary<string, object>();

            // Add authentication parameters
            var authParams = Config.GetAuthParams();
            foreach (var param in authParams)
            {
                parameters[param.Key] = param.Value;
            }

            var url = BASE_URL + endpoint;

            try
            {
                var content = new FormUrlEncodedContent(ConvertToStringDictionary(parameters));
                var response = await _httpClient.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                var data = JsonSerializer.Deserialize<Dictionary<string, object>>(responseString);
                _lastResponse = data ?? new Dictionary<string, object>();
                return _lastResponse;
            }
            catch (Exception ex)
            {
                _lastResponse = new Dictionary<string, object>
                {
                    { "status", false },
                    { "code", 0 },
                    { "msg", $"Request Error: {ex.Message}" },
                    { "response", null }
                };
                return null;
            }
        }

        /// <summary>
        /// Make API request (synchronous wrapper)
        /// </summary>
        public Dictionary<string, object> Request(string endpoint, Dictionary<string, object> parameters = null)
        {
            return RequestAsync(endpoint, parameters).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get last response
        /// </summary>
        /// <returns>Last response dictionary</returns>
        public Dictionary<string, object> GetLastResponse()
        {
            return _lastResponse;
        }

        /// <summary>
        /// Get response data only
        /// </summary>
        /// <returns>Response data or null</returns>
        public object GetResponseData()
        {
            return _lastResponse.TryGetValue("response", out var response) ? response : null;
        }

        /// <summary>
        /// Check if last request was successful
        /// </summary>
        /// <returns>True if successful, false otherwise</returns>
        public bool IsSuccess()
        {
            if (_lastResponse.TryGetValue("status", out var status))
            {
                if (status is JsonElement element)
                {
                    return element.GetBoolean();
                }
                return status is bool b && b;
            }
            return false;
        }

        /// <summary>
        /// Get error message from last response
        /// </summary>
        /// <returns>Error message or null if successful</returns>
        public string GetError()
        {
            if (IsSuccess()) return null;
            if (_lastResponse.TryGetValue("msg", out var msg))
            {
                return msg?.ToString() ?? "Unknown error";
            }
            return "Unknown error";
        }

        /// <summary>
        /// Convert Dictionary<string, object> to Dictionary<string, string>
        /// </summary>
        private static Dictionary<string, string> ConvertToStringDictionary(Dictionary<string, object> dict)
        {
            var result = new Dictionary<string, string>();
            foreach (var kvp in dict)
            {
                result[kvp.Key] = kvp.Value?.ToString() ?? "";
            }
            return result;
        }
    }
}
