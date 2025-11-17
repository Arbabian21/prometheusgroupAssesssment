// implementations of the interface

using alphavantageAPI.Models;
using System.Text.Json;


namespace alphavantageAPI.Services
{
    public class AlphaVantageIntradayDataService : IAlphaVantageIntradayDataService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        
        public AlphaVantageIntradayDataService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<List<IntradayEndpointDataShape>> GetIntradayDataAsync(string symbol)
        {
            var client = _httpClientFactory.CreateClient("AlphaVantage");

            var apiKey = _configuration["AlphaVantage:ApiKey"]; //from user secrets
            // logic for missing api key
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("API key is not configured.");
            }

            var url = $"?function=TIME_SERIES_INTRADAY&symbol={symbol}&interval=15min&apikey={apiKey}";
            
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonData = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(jsonData);
            var root = document.RootElement;
            // logic for unexpected response
            if (!root.TryGetProperty("Time Series (15min)", out var timeSeriesJson))
            {
                var error = root.TryGetProperty("Error Message", out var errProp) ? errProp.GetString() : null;
                throw new Exception("Expected 'Time Series (15min)' not found in Alpha Vantage response. Error: " + error);
            }

            var result = new List<IntradayEndpointDataShape>();
            var series = timeSeriesJson;;

            // logic to parse each interval in the time series
            foreach (var entry in series.EnumerateObject())
            {
                var timestamp = DateTime.Parse(entry.Name);
                var data = entry.Value;
                
                //5 min example structure
                // "Time Series (5min)": {
                //     "2009-01-30 19:55:00": {
                //         "1. open": "49.3475",
                //         "2. high": "49.3475",
                //         "3. low": "49.3475",
                //         "4. close": "49.3475",
                //         "5. volume": "209"
                //     },
                //     "2009-01-30 17:40:00": {
                //         "1. open": "49.3744",
                //         "2. high": "49.3744",
                //         "3. low": "49.3744",
                //         "4. close": "49.3744",
                //         "5. volume": "84726"
                //     },
                //     "2009-01-30 17:15:00": {
                //         "1. open": "49.6682",
                //         "2. high": "49.6682",
                //         "3. low": "49.6682",
                //         "4. close": "49.6682",
                //         "5. volume": "2651"
                //     }
                // }

                var dataShape = new IntradayEndpointDataShape
                {
                    Timestamp = timestamp,
                    Open = decimal.Parse(data.GetProperty("1. open").GetString()),
                    High = decimal.Parse(data.GetProperty("2. high").GetString()),
                    Low = decimal.Parse(data.GetProperty("3. low").GetString()),
                    Close = decimal.Parse(data.GetProperty("4. close").GetString()),
                    Volume = long.Parse(data.GetProperty("5. volume").GetString())
                };

                result.Add(dataShape);
            }

            return result; 
        }
    }
}