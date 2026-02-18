using System.ComponentModel.DataAnnotations;

namespace Umbra.School.Models.Assessment
{
    public class AssessmentInfoModel : BaseModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Category { get; set; } = string.Empty;
        [Required]
        public Int32 ExpectDuration { get; set; }
        public string? Description { get; set; }
        public string? ForUserIds { get; set; }
        public string? ForUserNames { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        [Required]
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? ReleaseDate { get; set; }
    }
}
