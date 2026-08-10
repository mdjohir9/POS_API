using POS_API.Entities.Master;
using System.ComponentModel.DataAnnotations;

namespace POS_API.Entities.Sales
{
    public class POSSalesPaymentMethod
    {
        [Required]
        [Key]
        public int Id { get; set; }


        public bool IsActive { get; set; } = true;


        public bool IsDeleted { get; set; } = false;


        [Required]
        public DateTime CreatedAt { get; set; }


        public int? CreatedBy { get; set; }


        public DateTime? UpdatedAt { get; set; }


        public int? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }


        public int? DeletedBy { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<POSSalesPayment> Payments { get; set; } = new List<POSSalesPayment>();
    }
}
