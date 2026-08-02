using System.ComponentModel.DataAnnotations;

namespace POS_API.Entities.Master
{
    public class POS_Category : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }


        public ICollection<POS_Product> Products { get; set; }
    }
}
