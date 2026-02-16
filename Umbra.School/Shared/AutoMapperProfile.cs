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
            //CreateMap<EnglishWord, EnglishWordModel>().ReverseMap();
            //CreateProjection<EnglishWord, EnglishWordModel>();            
            // Define the parameter name for use in ProjectTo
            string userId = null;
            CreateMap<EnglishWord, EnglishWordModel>()
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src =>
                    src.UserRatings
                        .Where(ur => ur.UserId == userId)
                        .Select(ur => ur.Rating)
                        .FirstOrDefault()));
            CreateMap<EnglishWordModel, EnglishWord>();

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
