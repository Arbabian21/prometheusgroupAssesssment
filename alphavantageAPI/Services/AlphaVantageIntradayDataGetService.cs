// implementations of the interface

using alphavantageAPI.Models;
using System.text.Json;


namespace alphavantageAPI.Services
{
    public class AlphaVantageIntradayDataGetService : IAlphaVantageIntradayDataGetService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Iconfiguration _configuration;
        
        public AlphaVantageIntradayDataGetService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<List<IntradayEndpointDataShape>> GetIntradayDataAsync(string symbol)
        {
            var client = _httpClientFactory.createClient("AlphaVantage");

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
            var series = timeSeriesJson.Value;

            // logic to parse each interval in the time series
            foreach (var entry in series.EnumerateObject())
            {
                var timestamp = DateTime.Parse(entry.Name);
                var data = entry.Value;

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