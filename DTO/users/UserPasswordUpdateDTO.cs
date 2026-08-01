namespace POS_API.DTO.users
{
    public class UserPasswordUpdateDTO
    {
        public string? OldUserPassword { get; set; }
        public string? NewUserPassword { get; set; }
    }
}
