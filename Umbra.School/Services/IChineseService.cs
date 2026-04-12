using Umbra.School.Models.Chinese;

namespace Umbra.School.Services
{
    public interface IChineseService
    {
        Task<List<CountByKeyWordModel>> GetChineseClassicalKeyWordsAsync();
        Task<List<ChineseClassicalModel>> GetChineseClassicalsAsync();
        Task<List<ChineseClassicalModel>> GetChineseClassicalsAsync(string keyword);

    }
}
