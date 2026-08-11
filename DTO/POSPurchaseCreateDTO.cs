using System.ComponentModel.DataAnnotations;

namespace POS_API.DTO
{
    public class POSPurchaseCreateDTO
    {
        [Required]
        public string? PurchaseNo { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public int SupplierId { get; set; }

        public List<POSPurchaseDetailDTO>? Details { get; set; }
    }
}
