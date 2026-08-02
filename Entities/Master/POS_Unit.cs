using System.ComponentModel.DataAnnotations;

namespace POS_API.Entities.Master
{
    public class POS_Unit : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }


        [MaxLength(20)]
        public string? ShortName { get; set; }


        public ICollection<POS_Product>? Products { get; set; }
    }
}
