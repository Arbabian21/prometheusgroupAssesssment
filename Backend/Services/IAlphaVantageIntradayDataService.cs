using alphavantageAPI.Models;

namespace alphavantageAPI.Services
{
    public interface IAlphaVantageIntradayDataService
    {
        Task<List<IntradayEndpointDataShape>> GetIntradayDataAsync(string symbol);
    }
}