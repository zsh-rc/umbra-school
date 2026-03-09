using Umbra.School.Data;

namespace Umbra.School.Models.Account
{
    public class ApplicationUserModel
    {
        public string Id { get; set; }
        public string? UserName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }
}
