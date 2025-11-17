using alphavantageAPI.Models;

namespace alphavantageAPI.Services
{
    public interface IAlphaVantageIntradayDataGetService
    {
        Task<List<IntradayEndpointDataShape>> GetIntradayDataAsync(string symbol);
    }
}