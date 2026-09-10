namespace POS_API.DTO
{
    public class POSDashboardSummaryDTO
    {
        public POSDashboardCardDTO Sales { get; set; } = new();
        public POSDashboardCardDTO Purchase { get; set; } = new();
        public POSDashboardCardDTO Profit { get; set; } = new();

        public int LowStockItems { get; set; }
    }
}
