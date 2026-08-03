using POS_API.Entities.Master;

namespace POS_API.Entities.Sales
{
    public class POS_SalesPaymentMethod :BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public ICollection<POS_SalesPayment> Payments { get; set; } = new List<POS_SalesPayment>();
    }
}
