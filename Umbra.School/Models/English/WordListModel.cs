namespace Umbra.School.Models.English
{
    public class WordListModel
    {
        public int TotalWordsOfBook { get; set; } // Total count of whole book
        public List<EnglishWordModel> EnglishWords { get; set; } = new List<EnglishWordModel>();
    }
}
