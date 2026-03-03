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
        public string FullName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName))
                    return "Unknown User";

                // Logic: If either name contains Chinese characters, use Chinese format
                if (UtilityHelper.ContainsChinese(FirstName) || UtilityHelper.ContainsChinese(LastName))
                {
                    return $"{LastName}{FirstName}"; // No space for Chinese names
                }

                return $"{FirstName} {LastName}"; // Space for English names
            }
        }
    }
}
