using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Umbra.School.Data;
using Umbra.School.Data.Dashboard;
using Umbra.School.Data.PersonalData;
using Umbra.School.Models;
using Umbra.School.Models.Assessment;
using Umbra.School.Models.Dashboard;
using Umbra.School.Models.English;
using Umbra.School.Shared;
using static MudBlazor.CategoryTypes;

namespace Umbra.School.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DashboardService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task DashboardDataStatistics()
        {
            try
            {
                // Statistic English Words Count
                // - Group by book and count words                
                var wordCounts = _context.EnglishWords
                    .GroupBy(w => w.Book)
                    .Select(g => new ReportEnWordsCountModel
                    {
                        Book = g.Key,
                        WordsCount = g.Count()
                    })
                    .ToList();
                // - Count unfamiliar words for each user and add to the list
                var unfamiliars = _context.Users
                    .Select(user => new ReportEnWordsCountModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        Book = "myu",
                        // Count only the ratings that meet your criteria (Rating <= 3)
                        WordsCount = _context.UserEnglishWordRatings
                            .Count(w => w.UserId == user.Id && w.Rating <= 3)
                    })
                    .ToList();
                wordCounts.AddRange(unfamiliars);
                // - Insert into ReportEnWordsCounts table
                List<ReportEnWordsCount> entities = _mapper.Map<List<ReportEnWordsCount>>(wordCounts);
                await _context.ReportEnWordsCounts.ExecuteDeleteAsync();
                _context.ReportEnWordsCounts.AddRange(entities);

                // Statistic Assessment Results
                // - Get all user assessments with related user and assessment info
                var userAssessments = _context.WordsAssessmentDetails
                    .Include(wad => wad.ApplicationUser)
                    .Include(wad => wad.WordsAssessment)
                    .Include(wad => wad.WordsAssessment!.AssessmentInfo)
                    .GroupBy(wad => new
                    {
                        UserId = wad.ApplicationUser!.Id,
                        wad.ApplicationUser.UserName,
                        AssessmentInfoId = wad.WordsAssessment!.AssessmentInfo!.Id,
                        AssessmentInfoName = wad.WordsAssessment!.AssessmentInfo!.Name
                    }).Select(g => new ReportUserAssessmentModel
                    {
                        UserId = g.Key.UserId,
                        UserName = g.Key.UserName!,
                        AssessmentInfoId = g.Key.AssessmentInfoId,
                        AssessmentInfoName = g.Key.AssessmentInfoName,
                        WordsCountInvolved = g.Count()
                    });
                // - Get all assessment results with related user and assessment info
                var userAssessmentResults = _context.AssessmentResults
                    .Include(ar => ar.ApplicationUser)
                    .Include(ar => ar.AssessementInfo)
                    .Select(ar => new
                    {
                        UserId = ar.ApplicationUser!.Id,
                        ar.ApplicationUser.UserName,
                        AssessmentInfoId = ar.AssessementInfo!.Id,
                        AssessmentInfoName = ar.AssessementInfo.Name,
                        ar.Date,
                        Score = ar.Score
                    });
                // - Left join user assessments with assessment results to get complete data for the report
                var finalResult = userAssessments.LeftJoin(
                    userAssessmentResults,
                    ua => new { ua.UserId, ua.AssessmentInfoId },
                    ur => new { ur.UserId, ur.AssessmentInfoId },
                    (ua, ur) => new ReportUserAssessmentModel
                    {
                        UserId = ua.UserId,
                        AssessmentInfoId = ua.AssessmentInfoId,
                        WordsCountInvolved = ua.WordsCountInvolved,
                        UserName = ua.UserName,
                        AssessmentInfoName = ua.AssessmentInfoName,
                        Date = (DateTime?)(ur != null ? ur.Date : null) ?? DateTime.MinValue,
                        Score = (decimal?)(ur != null ? ur.Score : null) ?? 0
                    });
                var result = finalResult.ToList();
                // - Insert into ReportUserAssessments table
                List<ReportUserAssessment> assessmentEntities = _mapper.Map<List<ReportUserAssessment>>(finalResult);
                await _context.ReportUserAssessments.ExecuteDeleteAsync();
                _context.ReportUserAssessments.AddRange(assessmentEntities);

                // Statistic User Assessment Status
                // - Get all user's assessments from detail table.
                var listStatus = new List<ReportUserAssessmentStatusModel>();
                var qryTotalUserAssessments = _context.WordsAssessmentDetails
                    .Select(wad => new
                    {
                        wad.UserId,
                        wad.ApplicationUser!.UserName,
                        wad.ApplicationUser!.Email,
                        wad.ApplicationUser!.FullName,
                        wad.WordsAssessementId
                    }).Distinct()
                    .GroupBy(wad => new
                    {
                        wad.UserId,
                        wad.UserName,
                        wad.Email,
                        wad.FullName
                    }).Select(g => new ReportUserAssessmentStatusModel
                    {
                        UserId = g.Key.UserId!,
                        UserName = g.Key.UserName!,
                        Email = g.Key.Email!,
                        FullName = g.Key.FullName!,
                        Total = g.Count()
                    }).ToList();
                listStatus.AddRange(qryTotalUserAssessments);
                // - Get all user's assessments with result status from result table.
                var qryUserAssessmentStatus = _context.AssessmentResults
                    .GroupBy(ar => new
                    {
                        UserId = ar.ApplicationUser!.Id,
                        ar.Status
                    }).Select(g => new
                    {
                        g.Key.UserId,
                        g.Key.Status,
                        AssessmentCount = g.Count()
                    }).ToList();
                qryUserAssessmentStatus.ForEach(status =>
                {
                    var item = listStatus.FirstOrDefault(u => u.UserId == status.UserId);
                    if (item == null)
                    {
                        item = new ReportUserAssessmentStatusModel
                        {
                            UserId = status.UserId,
                            Total = 0
                        };
                        listStatus.Add(item);
                    }
                    else if (status.Status == AssessmentStatus.InProgress.GetDescription())
                        item.InProgress = status.AssessmentCount;
                    else if (status.Status == AssessmentStatus.Submitted.GetDescription())
                        item.Submitted = status.AssessmentCount;
                    else if (status.Status == AssessmentStatus.Reviewed.GetDescription())
                        item.Reviewed = status.AssessmentCount;
                });
                // - Update NotStarted count by subtracting InProgress, Submitted and Reviewed counts from Total count
                listStatus.ForEach(u =>
                {
                    u.NotStarted = u.Total - u.InProgress - u.Submitted - u.Reviewed;
                });
                // - Save to ReportUserAssessmentStatuses table
                List<ReportUserAssessmentStatus> statusEntities = _mapper.Map<List<ReportUserAssessmentStatus>>(listStatus);
                await _context.ReportUserAssessmentStatuses.ExecuteDeleteAsync();
                _context.ReportUserAssessmentStatuses.AddRange(statusEntities);

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ReportEnWordsCountModel>> GetReportEnWordsCounts(string userId)
        {
            var query = _context.ReportEnWordsCounts.Where(r => string.IsNullOrEmpty(r.UserId) || r.UserId == userId);
            var list = _mapper.ProjectTo<ReportEnWordsCountModel>(query).ToList();
            return list;
        }

        public async Task<List<ReportUserAssessmentModel>> GetReportUserAssessments(string userId)
        {
            var query = _context.ReportUserAssessments.Where(r => r.UserId == userId && r.Date != DateTime.MinValue);
            var list = _mapper.ProjectTo<ReportUserAssessmentModel>(query).ToList();
            return list;
        }

        public async Task<ReportUserAssessmentStatusModel?> GetReportUserAssessmentStatuses(string userId)
        {
            var query = _context.ReportUserAssessmentStatuses.Where(r => r.UserId == userId);
            var list = _mapper.ProjectTo<ReportUserAssessmentStatusModel>(query).ToList();
            return list.FirstOrDefault();
        }
    }
}
