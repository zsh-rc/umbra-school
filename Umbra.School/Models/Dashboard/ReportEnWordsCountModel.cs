namespace Umbra.School.Models.Dashboard
{
    public class ReportEnWordsCountModel
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string Book { get; set; } = string.Empty;
        public int WordsCount { get; set; }
    }
}
