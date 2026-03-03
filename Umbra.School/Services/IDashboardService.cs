using Umbra.School.Models;
using Umbra.School.Models.Assessment;
using Umbra.School.Models.Dashboard;

namespace Umbra.School.Services
{
    public interface IDashboardService
    {
        Task<ResponseModel<List<EnglishWordsCountModel>>> GetEnglishWordsCount(string userId);
        Task<ResponseModel<List<EnglishWordsAssessmentResultModel>>> GetAssessmentResults(string userId);
    }
}
