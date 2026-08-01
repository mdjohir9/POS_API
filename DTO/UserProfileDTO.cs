namespace POS_API.DTO
{
    public class UserProfileDTO
    {

        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }

        public string? UserImage { get; set; }
        public string? UserPassword { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int UserRoleID { get; set; }
        public string? UserRoleName { get; set; }
        public Boolean? IsGuestUser { get; set; }
        public bool? IsApprovingAuthority { get; set; }
        public string? ReferenceID { get; set; }
        public string? AdditionalPermissions { get; set; }
        public string? RemovedPermissions { get; set; }
        public int? DataAccessLevel { get; set; }
        public List<string> DataAccessPermission { get; set; }  // Change to List<string>
        public Boolean? IsActive { get; set; }
        public string? CompanyId { get; set; }

        public int? LoanId { get; set; }

    }
}
