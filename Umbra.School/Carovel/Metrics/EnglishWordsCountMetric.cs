using Coravel.Pro.Features.Metrics;
using Microsoft.EntityFrameworkCore;
using Umbra.School.Data;

namespace Umbra.School.Carovel.Metrics
{
    public class EnglishWordsCountMetric : IMetricCard
    {
        private ApplicationDbContext _context;

        // Inject dbContext from service provider.
        public EnglishWordsCountMetric(ApplicationDbContext context) =>
            this._context = context;

        // Fetch and assign values in this method.  
        public async Task ExecuteAsync()
        {
            int count = await this._context.EnglishWords.CountAsync();
            this.Value = $"{count}个单词";
            this.Title = "英语词汇";
            this.IsPositiveMetric = true;
            this.IncludeArrow = true;
        }

        // These are implemented from IMetric Card.
        public string Value { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public bool IsPositiveMetric { get; set; }
        public bool IncludeArrow { get; set; }
    }
}
