using Coravel.Invocable;
using Umbra.School.Data;
using Umbra.School.Services;

namespace Umbra.School.Carovel.Jobs
{
    public class DashbordStatisticsJob : IInvocable
    {
        private readonly IDashboardService _dashboardService;

        public DashbordStatisticsJob(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task Invoke()
        {
           await _dashboardService.DashboardDataStatistics();
        }
    }
}
