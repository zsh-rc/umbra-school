using System;
using System.Collections.Generic;
using System.Text;

namespace Umbra.School.Data.Dashboard
{
    public class ReportUserAssessmentStatus : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public int Total { get; set; }
        public int NotStarted { get; set; }
        public int InProgress { get; set; }
        public int Submitted { get; set; }
        public int Reviewed { get; set; }
    }
}
