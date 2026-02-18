using System.ComponentModel.DataAnnotations;
using Umbra.School.Data;
using Umbra.School.Models.English;

namespace Umbra.School.Models.Assessment
{
    public class WordsAssessmentDetailModel : BaseEntity
    {
        public string? UserId { get; set; } 
        [Required]
        public Guid WordId { get; set; }
        public EnglishWordModel? EnglishWord { get; set; }
        [Required]
        public string Word { get; set; } = string.Empty;
        [Required]
        public Guid WordsAssessementId { get; set; }
        public WordsAssessmentModel? WordsAssessment { get; set; }
        [Required]
        public string Meaning { get; set; } = string.Empty;
        
        public string? Answer { get; set; } = string.Empty;
        public bool? Correct {  get; set; }
    }
}
