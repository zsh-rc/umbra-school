using Umbra.School.Models;
using Umbra.School.Models.Assessment;
using Umbra.School.Models.English;

namespace Umbra.School.Services
{
    public interface IAssessmentService
    {
        Task<ResponseModel<List<WordsAssessmentModel>>?> GetWordsAssessments();
        Task<ResponseModel<List<WordsAssessmentModel>>?> GetWordsAssessments(string userId);
        Task<ResponseModel<bool>> AddWordsAssessment(WordsAssessmentModel model);
        Task<ResponseModel<List<WordsAssessmentDetailModel>>> GetWordsAssessmentDetails(string userId, Guid assessmentInfoId);
        Task<ResponseModel<bool>> SubmitAssessmentAnswers(List<WordsAssessmentDetailModel> answers);
        Task<ResponseModel<bool>> StartAssessment(Guid assessmentInfoId, string userId);
        Task<ResponseModel<bool>> EndAssessment(Guid assessmentInfoId, string userId);
        Task<ResponseModel<List<AssessmentResultModel>>> GetAssessmentResults(string category);
        Task<ResponseModel<bool>> RectifyAssessmentDetailRecord(Guid id, bool correct);
        Task<ResponseModel<bool>> SubmitReviewResult(AssessmentResultModel review);
    }
}
