using System;
using System.Collections.Generic;
using System.Text;
using Umbra.School.Models;

namespace Umbra.School.Models.Dashboard
{
    public class ReportUserAssessmentModel : BaseModel
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public Guid AssessmentInfoId { get; set; }
        public string AssessmentInfoName { get; set; } = string.Empty;
        public int WordsCountInvolved { get; set; }
        public decimal Score { get; set; }
    }
}
