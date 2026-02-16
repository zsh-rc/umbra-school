using AutoMapper;
using Umbra.School.Data.Blog;
using Umbra.School.Data.Chinese;
using Umbra.School.Data.English;
using Umbra.School.Models.Chinese;
using Umbra.School.Models.English;

namespace Umbra.School.Shared
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            #region English
            CreateMap<EnglishWord, EnglishWordModel>().ReverseMap();
            CreateProjection<EnglishWord, EnglishWordModel>();
            CreateMap<EnglishPhrase, EnglishPhraseModel>().ReverseMap();
            CreateProjection<EnglishPhrase, EnglishPhraseModel>();
            CreateMap<EnglishTranslation, EnglishTranslationModel>().ReverseMap();
            CreateProjection<EnglishTranslation, EnglishTranslationModel>();
            #endregion
            #region Chinese
            CreateMap<ChineseClassicalQuestion, ChineseClassicalQuestionModel>().ReverseMap();
            CreateProjection<ChineseClassicalQuestion, ChineseClassicalQuestionModel>();
            #endregion
        }
    }
}
