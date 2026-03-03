using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Umbra.School.Data;
using Umbra.School.Models;
using Umbra.School.Models.Assessment;
using Umbra.School.Models.Dashboard;
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

        public Task<ResponseModel<List<EnglishWordsAssessmentResultModel>>> GetAssessmentResults(string userId)
        {
            var statistics = _context.AssessmentResults
                .Where(r => r.UserId == userId)
                .Include(r => r.ApplicationUser)
            .Join(_context.AssessmentInfos,
                result => result.AssessmentInfoId,
                info => info.Id,
                (result, info) => new { result, info })
            .Join(_context.WordsAssessments,
                combined => combined.info.Id,
                wa => wa.AssessmentInfoId,
                (combined, wa) => new
                {
                    combined.result.UserId,
                    combined.result.ApplicationUser.UserName,
                    AssessmentInfoId = combined.info.Id,
                    AssessmentInfoName = combined.info.Name,
                    combined.result.Date,
                    combined.result.Score,
                    WordsAssessmentId = wa.Id
                })
            .Join(_context.WordsAssessmentDetails,
                flat => flat.WordsAssessmentId,
                detail => detail.WordsAssessementId,
                (flat, detail) => new
                {
                    flat.UserId,
                    flat.UserName,
                    flat.AssessmentInfoId,
                    flat.AssessmentInfoName,
                    flat.Date,
                    flat.Score,
                    detail.WordId
                })
            .GroupBy(x => new { x.UserId, x.UserName, x.AssessmentInfoId, x.AssessmentInfoName, x.Date, x.Score })
            .Select(g => new EnglishWordsAssessmentResultModel
            {
                UserId = g.Key.UserId,
                UserName = g.Key.UserName,
                AssessmentInfoId = g.Key.AssessmentInfoId,
                AssessmentInfoName = g.Key.AssessmentInfoName,
                Date = g.Key.Date,
                Score = g.Key.Score,
                WordsCount = g.Count()
            })
            .ToList();

            var result = new ResponseModel<List<EnglishWordsAssessmentResultModel>>()
            {
                Success = true,
                Data = statistics,
                Code = "DATA-LOADED",
                Message = "Data loaded successfully."
            };
            return Task.FromResult(result);
        }

        public Task<ResponseModel<List<EnglishWordsCountModel>>> GetEnglishWordsCount(string userId)
        {
            var wordCounts = _context.EnglishWords
                .GroupBy(w => w.Book)
                .Select(g => new EnglishWordsCountModel
                {
                    Book = g.Key,
                    WordsCount = g.Count()
                })
                .ToList();

            var unfamiliars = _context.UserEnglishWordRatings.Where(e => e.UserId == userId && e.Rating <= 3).ToList();
            wordCounts.Add(new EnglishWordsCountModel { Book = "myu", WordsCount = unfamiliars.Count });

            var result = new ResponseModel<List<EnglishWordsCountModel>>()
            {
                Success = true,
                Data = wordCounts,
                Code = "WORDS-COUNT-LOADED",
                Message = "Data loaded successfully."
            };
            return Task.FromResult(result);
        }
    }
}
