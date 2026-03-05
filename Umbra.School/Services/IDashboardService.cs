using Umbra.School.Models;
using Umbra.School.Models.Assessment;
using Umbra.School.Models.Dashboard;

namespace Umbra.School.Services
{
    public interface IDashboardService
    {
        Task DashboardDataStatistics();
        Task<List<ReportEnWordsCountModel>> GetReportEnWordsCounts(string userId);
        Task<List<ReportUserAssessmentModel>> GetReportUserAssessments(string userId);
    }
}
