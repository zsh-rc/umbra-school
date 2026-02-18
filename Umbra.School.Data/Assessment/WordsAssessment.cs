using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Umbra.School.Data.Notebook;

namespace Umbra.School.Data.Assessment
{
    public class WordsAssessment : BaseEntity
    {
        [Required]
        public Guid AssessmentInfoId { get; set; }
        [ForeignKey("AssessmentInfoId")]
        public AssessmentInfo? AssessmentInfo { get; set; }
        [Required]
        public string Scope { get; set; } = string.Empty;
        [Required]
        public string Source { get; set; } = string.Empty;
        public Guid? NotebookInfoId { get; set; }
        [ForeignKey("NotebookInfoId")]
        public NotebookInfo? NotbookInfo { get; set; }
        [Required]
        public Int32 WordCount { get; set; }
        [Required]
        public string Method { get; set; } = string.Empty;
        public Int32 StartIndex { get; set; } = -1;
    }
}
