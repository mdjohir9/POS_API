using System.ComponentModel.DataAnnotations;

namespace POS_API.Entities.Master
{
    public class POSUnit 
    {
        [Required]
        [Key]
        public int Id { get; set; }


        public bool IsActive { get; set; } = true;


        public bool IsDeleted { get; set; } = false;


        [Required]
        public DateTime CreatedAt { get; set; }


        public int? CreatedBy { get; set; }
        public int? CompanyId { get; set; }

        public DateTime? UpdatedAt { get; set; }


        public int? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }


        public int? DeletedBy { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }


        [MaxLength(20)]
        public string? ShortName { get; set; }


        public ICollection<POSProduct>? Products { get; set; }
    }
}
