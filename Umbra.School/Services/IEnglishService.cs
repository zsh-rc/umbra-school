using Umbra.School.Models;
using Umbra.School.Models.English;

namespace Umbra.School.Services
{
    public interface IEnglishService
    {
        Task<ResponseModel<List<EnglishWordModel>>?> GetEnglishWords();
        Task<ResponseModel<WordListModel>> GetEnglishWords(string? book, string? alphabet);
        Task<ResponseModel<WordListModel>> GetEnglishWordsWithRatings(string username, string? book, string? alphabet, int? start, int? end);
        Task<ResponseModel<WordListModel>> GetUnfamiliarEnglishWords(string userId, string? alphabet, int? start, int? end);
        Task<ResponseModel<EnglishWordModel>?> GetEnglishWordById(int id);
        Task<ResponseModel<Guid>?> AddEnglishWord(EnglishWordModel word);
        Task<ResponseModel<bool>?> UpdateEnglishWord(EnglishWordModel word);
        Task<ResponseModel<bool>?> DeleteEnglishWord(int id);
        Task<ResponseModel<bool>> RatingEnglishWord(string userId, Guid wordId, int rating);
    }
}
