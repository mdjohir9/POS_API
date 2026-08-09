using POS_API.Entities.Master;
using System.ComponentModel.DataAnnotations;

namespace POS_API.Entities.Sales
{
    public class POSSalesPaymentMethod
    {
        [Required]
        [Key]
        public long Id { get; set; }


        public bool IsActive { get; set; } = true;


        public bool IsDeleted { get; set; } = false;


        [Required]
        public DateTime CreatedAt { get; set; }


        public long? CreatedBy { get; set; }


        public DateTime? UpdatedAt { get; set; }


        public long? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }


        public long? DeletedBy { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<POSSalesPayment> Payments { get; set; } = new List<POSSalesPayment>();
    }
}
