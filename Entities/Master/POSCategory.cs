using System.ComponentModel.DataAnnotations;

namespace POS_API.Entities.Master
{
    public class POSCategory 
    {
        [Required]
        [Key]
        public long Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        public bool IsActive { get; set; } = true;


        public bool IsDeleted { get; set; } = false;


        [Required]
        public DateTime CreatedAt { get; set; }


        public long? CreatedBy { get; set; }


        public DateTime? UpdatedAt { get; set; }


        public long? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }


        public long? DeletedBy { get; set; }


        public ICollection<POSProduct>? Products { get; set; }
    }
}
