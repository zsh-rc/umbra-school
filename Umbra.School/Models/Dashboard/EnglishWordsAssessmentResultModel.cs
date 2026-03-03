namespace Umbra.School.Models.Dashboard
{
    public class EnglishWordsAssessmentResultModel
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public Guid AssessmentInfoId { get; set; }
        public string AssessmentInfoName { get; set; } = string.Empty;
        public int WordsCount { get; set; }
        public decimal? Score { get; set; }
    }
}
