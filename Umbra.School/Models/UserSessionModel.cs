using Umbra.School.Shared;

namespace Umbra.School.Models
{
    public class UserSessionModel
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set;} = string.Empty; 
        public bool IsAdmin { get; set; } = false;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }
}
