using POS_API.Entities.Master;

namespace POS_API.Entities.Sales
{
    public class POSSalesPaymentMethod :BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public ICollection<POSSalesPayment> Payments { get; set; } = new List<POSSalesPayment>();
    }
}
