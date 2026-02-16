using Umbra.School.Models;
using Umbra.School.Models.English;

namespace Umbra.School.Services
{
    public interface IEnglishService
    {
        Task<ResponseModel<List<EnglishWordModel>>?> GetEnglishWords();
        Task<ResponseModel<List<EnglishWordModel>>?> GetEnglishWordsByAlphabet(string alphabet);
        Task<ResponseModel<EnglishWordModel>?> GetEnglishWordById(int id);
        Task<ResponseModel<Guid>?> AddEnglishWord(EnglishWordModel word);
        Task<ResponseModel<bool>?> UpdateEnglishWord(EnglishWordModel word);
        Task<ResponseModel<bool>?> DeleteEnglishWord(int id);
    }
}
