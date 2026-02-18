using AutoMapper;
using Umbra.School.Data;
using Umbra.School.Data.Assessment;
using Umbra.School.Data.Blog;
using Umbra.School.Data.Chinese;
using Umbra.School.Data.English;
using Umbra.School.Data.Notebook;
using Umbra.School.Models.Account;
using Umbra.School.Models.Assessment;
using Umbra.School.Models.Chinese;
using Umbra.School.Models.English;
using Umbra.School.Models.Notebook;

namespace Umbra.School.Shared
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            #region Account
            CreateMap<ApplicationUser, ApplicationUserModel>().ReverseMap();
            CreateProjection<ApplicationUser, ApplicationUserModel>();
            #endregion

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
            #region Assessment
            CreateMap<AssessmentInfo, AssessmentInfoModel>().ReverseMap();
            CreateProjection<AssessmentInfo, AssessmentInfoModel>();
            CreateMap<AssessmentResult, AssessmentResultModel>().ReverseMap();
            CreateProjection<AssessmentResult, AssessmentResultModel>();
            CreateMap<WordsAssessment, WordsAssessmentModel>().ReverseMap();
            CreateProjection<WordsAssessment, WordsAssessmentModel>();
            CreateMap<WordsAssessmentDetail, WordsAssessmentDetailModel>().ReverseMap();
            CreateProjection<WordsAssessmentDetail, WordsAssessmentDetailModel>();
            #endregion
            #region Notebook
            CreateMap<NotebookInfo, NotebookInfoModel>().ReverseMap();
            CreateProjection<NotebookInfo, NotebookInfoModel>();
            #endregion
        }
    }
}
