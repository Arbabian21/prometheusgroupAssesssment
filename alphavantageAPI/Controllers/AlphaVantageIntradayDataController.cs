using Microsoft.AspNetCore.Mvc;
using alphavantageAPI.Services;
using alphavantageAPI.Models;
using System.linq;

namespace alphavantageAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlphaVantageIntradayDataController : ControllerBase
    {
        private readonly IAlphaVantageIntradayDataService _intradayDataService;

        // inject the service
        public AlphaVantageIntradayDataController(IAlphaVantageIntradayDataService intradayDataService)
        {
            _intradayDataService = intradayDataService;
        }

        // GET route
        [HttpGet("summary")]
        public async Task<ActionResult<List<DaySummaryDataShape>>> GetIntradayData([FromQuery] string symbol)
        {
            // logic for no symbol provided
            if (string.IsNullOrEmpty(symbol))
            {
                return BadRequest("Symbol query parameter is required.");
            }

            try
            {
                var data = await _intradayDataService.GetIntradayDataAsync(symbol);

                // filter to last month
                var oneMonthAgo = DateTime.UtcNow.AddMonths(-1); //bc for some reason utc is standard here
                var lastMonthData = data.Where(d => d.Timestamp.ToUniversalTime() >= oneMonthAgo);

                // gropu data by day
                var groupedData = lastMonthData
                    .GroupBy(d => d.Timestamp.Date)
                    .OrderBy(d => d.Key)
                    .Select(g => new DaySummaryDataShape
                    {
                    Day = g.Key.ToString("yyyy-MM-dd"),
                    LowAverage = g.Average(x => (double)x.Low),
                    HighAverage = g.Average(x => (double)x.High),
                    Volume = g.Sum(x => x.Volume)
                    })
                    .ToList();
                
                return Ok(groupedData);
            }
            catch (Exception ex)
            {
                //exception
                return StatusCode(500, "An error occurred while processing your request: " + ex.Message);
            }
        }
    }
}