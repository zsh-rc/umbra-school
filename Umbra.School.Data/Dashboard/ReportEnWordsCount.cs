using System;
using System.Collections.Generic;
using System.Text;

namespace Umbra.School.Data.Dashboard
{
    public class ReportEnWordsCount : BaseEntity
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string Book { get; set; } = string.Empty;
        public int WordsCount { get; set; }
    }
}
