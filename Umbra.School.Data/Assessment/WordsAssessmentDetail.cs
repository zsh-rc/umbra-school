using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Umbra.School.Data.English;

namespace Umbra.School.Data.Assessment
{
    public class WordsAssessmentDetail : BaseEntity
    {
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }
        [Required]
        public Guid WordId { get; set; }
        [ForeignKey("WordId")]
        public EnglishWord? EnglishWord { get; set; }
        [Required]
        public string Word { get; set; } = string.Empty;
        [Required]
        public Guid WordsAssessementId { get; set; }
        [ForeignKey("WordsAssessementId")]
        public WordsAssessment? WordsAssessment { get; set; }
        [Required]
        public string Meaning { get; set; } = string.Empty;

        public string? Answer { get; set; } = string.Empty;
        public bool? Correct { get; set; }
    }
}
