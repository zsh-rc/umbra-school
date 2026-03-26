using Umbra.School.Data;

namespace Umbra.School.Models.Account
{
    public class ApplicationUserModel
    {
        public string Id { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTimeOffset? LockoutEnd { get; set; }
        public DateTime? PasswordExpiredTime { get; set; }
        public string? Avatar { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber {  get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }

    }
}
