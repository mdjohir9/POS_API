using System.ComponentModel.DataAnnotations;

namespace POS_API.Entities.Master
{
    public class POSBrand : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }


        public ICollection<POSProduct>? Products { get; set; }
    }
}
