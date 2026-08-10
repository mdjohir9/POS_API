using System.ComponentModel.DataAnnotations;

namespace POS_API.Entities.Master
{
    public class POSSupplier 
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public int? CompanyId { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        [Required]
        public DateTime CreatedAt { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? DeletedAt { get; set; }


        public int? DeletedBy { get; set; }

        [Required]
        [MaxLength(50)]
        public string? SupplierCode { get; set; }



        [Required]
        [MaxLength(150)]
        public string? SupplierName { get; set; }



        [MaxLength(20)]
        public string? Phone { get; set; }



        [MaxLength(300)]
        public string? Address { get; set; }
    }
}
