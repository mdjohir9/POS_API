namespace POS_API.DTO
{
    public class SalesPurchaseSummaryDTO
    {
        public decimal SalesAmount { get; set; }
        public decimal PurchaseAmount { get; set; }
        public decimal[] MonthlySalesAmounts { get; set; } = new decimal[12];
        public decimal[] MonthlyPurchaseAmounts { get; set; } = new decimal[12];
    }
}
