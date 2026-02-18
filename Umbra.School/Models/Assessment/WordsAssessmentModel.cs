using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Umbra.School.Data.Assessment;
using Umbra.School.Data.Notebook;
using Umbra.School.Models.Notebook;

namespace Umbra.School.Models.Assessment
{
    public class WordsAssessmentModel : BaseModel
    {
        [Required]
        public Guid AssessmentInfoId { get; set; }
        [Required]
        public string AssessmentInfoName {  get; set; } = string.Empty;
        public AssessmentInfoModel AssessmentInfo { get; set; } = new AssessmentInfoModel();
        [Required]
        public string Scope { get; set; } = string.Empty;
        [Required]
        public string Source { get; set; } = string.Empty;
        public Guid? NotebookInfoId { get; set; }
        public NotebookInfo? NotbookInfo { get; set; }
        public NotebookInfoModel? NotebookInfo { get; set; } = new NotebookInfoModel();
        [Required]
        public Int32 WordCount { get; set; }
        [Required]
        public string Method { get; set; } = string.Empty;
        public Int32 StartIndex { get; set; } = -1;
        public Int32 ExpectDuration { get; set; }
        public List<WordsAssessmentDetailModel> Details { get; set; } = new List<WordsAssessmentDetailModel>();
    }
}
