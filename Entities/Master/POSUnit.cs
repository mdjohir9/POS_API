using System.ComponentModel.DataAnnotations;

namespace POS_API.Entities.Master
{
    public class POSUnit : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }


        [MaxLength(20)]
        public string? ShortName { get; set; }


        public ICollection<POSProduct>? Products { get; set; }
    }
}
