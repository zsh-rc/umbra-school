using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Umbra.School.Data;
using Umbra.School.Models.Chinese;

namespace Umbra.School.Services
{
    public class ChineseService : IChineseService
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ChineseService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<List<CountByKeyWordModel>> GetChineseClassicalKeyWordsAsync()
        {
            var pinyinComparer = StringComparer.Create(new CultureInfo("zh-CN"), CompareOptions.StringSort);
            var list = _context.ChineseClassicals
                .GroupBy(x => x.Keyword)
                .Select(g => new CountByKeyWordModel
                {
                    Keyword = g.Key,
                    Count = g.Count()
                })
                .AsEnumerable()  // Switch to client-side evaluation for custom comparer
                .OrderBy(x => x.Keyword, pinyinComparer)
                .ToList();

            return Task.FromResult(list);
        }

        public Task<List<ChineseClassicalModel>> GetChineseClassicalsAsync()
        {
            var list = _mapper.ProjectTo<ChineseClassicalModel>(_context.ChineseClassicals
             .OrderBy(e => e.Keyword)).ToList();

            return Task.FromResult(list);
        }

        public Task<List<ChineseClassicalModel>> GetChineseClassicalsAsync(string keyword)
        {
            var list = _mapper.ProjectTo<ChineseClassicalModel>(
                _context.ChineseClassicals
                .Where(cc => cc.Keyword.ToLower() == keyword.ToLower())
                .OrderBy(e => e.KeywordMeaning)).ToList();

            return Task.FromResult(list);
        }
    }
}
