namespace POS_API.DTO.UserRoles
{
    public class UserRolesInfoDTO
    {
        public int UserRoleId { get; set; }
        public string? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? UserRoleName { get; set; }
        public string? Permissions { get; set; }
        public int? DataAccessLevel { get; set; }
        public int Ordering { get; set; }
        public Boolean? IsActive { get; set; }
        public Boolean? Deleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }
    }

}
