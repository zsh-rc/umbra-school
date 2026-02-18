using Umbra.School.Models;
using Umbra.School.Models.Assessment;
using Umbra.School.Models.English;

namespace Umbra.School.Services
{
    public interface IAssessmentService
    {
        Task<ResponseModel<List<WordsAssessmentModel>>?> GetWordsAssessments();
        Task<ResponseModel<Guid?>> AddWordsAssessment(WordsAssessmentModel model);
    }
}
