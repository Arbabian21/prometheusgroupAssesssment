namespace alphavantageAPI.Models
{
    public class DaySummaryDataShape
    {
        public string Day { get; set; } = string.Empty;
        public double lowAverage { get; set; }
        public double highAverage { get; set; }
        public long Volume { get; set; } // sum of volumes for each 15min interval per day
    }
}