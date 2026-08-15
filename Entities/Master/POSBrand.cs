using System.ComponentModel.DataAnnotations;

namespace POS_API.Entities.Master
{
    [SoftDelete]
    public class POSBrand 
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        public bool IsActive { get; set; } = true;


        public bool IsDeleted { get; set; } = false;


        [Required]
        public DateTime CreatedAt { get; set; }


        public int? CreatedBy { get; set; }


        public DateTime? UpdatedAt { get; set; }


        public int? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }


        public int? DeletedBy { get; set; }

        public int? CompanyId { get; set; }
        

        public ICollection<POSProduct>? Products { get; set; }
    }
}
