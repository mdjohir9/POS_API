using POS_API.Entities.Master;
using System.ComponentModel.DataAnnotations;

namespace POS_API.Entities
{
    public class HrdCompanyInfo
    {
        [Key]
        [Required]
        public long Id { get; set; }

        public bool IsActive { get; set; } = true;


        public bool IsDeleted { get; set; } = false;

        [Required]
        public DateTime CreatedAt { get; set; }


        public long? CreatedBy { get; set; }


        public DateTime? UpdatedAt { get; set; }


        public long? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }


        public long? DeletedBy { get; set; }
        public bool? CompanyType { get; set; }

        public string? HeadOfficeId { get; set; }

        public string? CompanyName { get; set; }

        public string? CompanyNameBangla { get; set; }

        public string? Address { get; set; }

        public string? AddressBangla { get; set; }

        public string? Country { get; set; }

        public string? Telephone { get; set; }

        public string? Fax { get; set; }

        public string? DefaultCurrency { get; set; }

        public short? BusinessType { get; set; }

        public bool? MultipleBranch { get; set; }

        public string? Comments { get; set; }

        public string? CompanyLogo { get; set; }

        public string? StartCardNo { get; set; }

        public string? Weekend { get; set; }

        public string? ShortName { get; set; }

        public bool? CardNoType { get; set; }

        public short? FlatCode { get; set; }

        public short? CardNoDigits { get; set; }

        public string? AttMachineName { get; set; }

        public DateOnly? PfcountDate { get; set; }

        public bool? IsLeaveAuthority { get; set; }

        public bool? IsOdauthority { get; set; }
        public byte? Status { get; set; }
        public string? Email { get; set; }

      }
}
