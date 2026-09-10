namespace POS_API.DTO
{
    public class StockInOutSummaryDTO
    {
        public decimal StockInTotal { get; set; }
        public decimal StockOutTotal { get; set; }
        public List<string> Labels { get; set; } = new();
        public List<decimal> StockInData { get; set; } = new();
        public List<decimal> StockOutData { get; set; } = new();
    }
}
