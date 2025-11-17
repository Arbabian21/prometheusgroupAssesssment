namespace alphavantageAPI.Models
{
    public class DaySummaryDataShape
    {
        public string Day { get; set; }
        public double lowAvg { get; set; }
        public double highAvg { get; set; }
        public long Volume { get; set; } // sum of volumes for each 15min interval per day
    }
}