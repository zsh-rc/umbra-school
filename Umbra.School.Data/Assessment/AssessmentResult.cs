using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Umbra.School.Data.Assessment
{
    public class AssessmentResult : BaseEntity
    {
        [Required]
        public string UserId { get; set; } = Guid.NewGuid().ToString();
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }
        [Required]
        public Guid AssessmentInfoId { get; set; }
        [ForeignKey("AssessmentInfoId")]
        public AssessmentInfo? AssessementInfo { get; set; }
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
