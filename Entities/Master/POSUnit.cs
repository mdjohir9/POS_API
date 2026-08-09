using System.ComponentModel.DataAnnotations;

namespace POS_API.Entities.Master
{
    public class POSUnit 
    {
        [Required]
        [Key]
        public long Id { get; set; }


        public bool IsActive { get; set; } = true;


        public bool IsDeleted { get; set; } = false;


        [Required]
        public DateTime CreatedAt { get; set; }


        public long? CreatedBy { get; set; }
        public long? CompanyId { get; set; }

        public DateTime? UpdatedAt { get; set; }


        public long? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }


        public long? DeletedBy { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }


        [MaxLength(20)]
        public string? ShortName { get; set; }


        public ICollection<POSProduct>? Products { get; set; }
    }
}
