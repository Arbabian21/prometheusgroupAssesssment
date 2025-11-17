namespace alphavantageAPI.Models
{
    public class IntradayEndpointDataShape
    {
        public DateTime Timestamp { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public long Volume { get; set; } // volume specifically for the 15min interval
    }
}