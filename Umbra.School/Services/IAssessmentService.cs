using Umbra.School.Models;
using Umbra.School.Models.Assessment;
using Umbra.School.Models.English;

namespace Umbra.School.Services
{
    public interface IAssessmentService
    {
        Task<ResponseModel<List<WordsAssessmentModel>>?> GetWordsAssessments();
        Task<ResponseModel<List<WordsAssessmentModel>>?> GetWordsAssessments(string userId);
        Task<ResponseModel<Guid?>> AddWordsAssessment(WordsAssessmentModel model);
        Task<ResponseModel<List<WordsAssessmentDetailModel>>> GetWordsAssessmentDetail(string userId, Guid wordAssessmentId);
        Task<ResponseModel<bool>> SubmitAssessmentAnswers(List<WordsAssessmentDetailModel> answers);
        Task<ResponseModel<bool>> StartAssessment(Guid assessmentInfoId, string userId);
        Task<ResponseModel<bool>> EndAssessment(Guid assessmentInfoId, string userId);
    }
}
