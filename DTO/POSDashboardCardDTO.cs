namespace POS_API.DTO
{
    public class POSDashboardCardDTO
    {
        public decimal Value { get; set; }
        public decimal Growth { get; set; }
        public bool IsUp { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
