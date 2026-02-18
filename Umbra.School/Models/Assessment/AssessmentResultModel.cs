using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Umbra.School.Models.Assessment
{
    public class AssessmentResultModel : BaseModel
    {
        [Required]
        public string UserId { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public Guid AssessmentInfoId { get; set; }
        public AssessmentInfoModel AssessementInfo { get; set; } = new AssessmentInfoModel();
        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        [Precision(18, 2)]
        public decimal? Score { get; set; }
        public string? Feedback { get; set; }
        public DateTime? ReviewDate { get; set; }
        public string? ReviewedBy { get; set; }
    }
}
