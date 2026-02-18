using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Umbra.School.Data;
using Umbra.School.Data.Assessment;
using Umbra.School.Data.Blog;
using Umbra.School.Data.English;
using Umbra.School.Models;
using Umbra.School.Models.Account;
using Umbra.School.Models.Assessment;

namespace Umbra.School.Services
{
    public class AssessmentService : IAssessmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AssessmentService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseModel<Guid?>> AddWordsAssessment(WordsAssessmentModel model)
        {
            try
            {
                var entity = _mapper.Map<WordsAssessment>(model);
                var assessmentInfo = entity.AssessmentInfo;
                if (assessmentInfo == null)
                {
                    throw new Exception("No assessment information found.");
                }
                assessmentInfo.Id = Guid.NewGuid();
                await _context.AssessmentInfos.AddAsync(assessmentInfo);
                entity.AssessmentInfoId = assessmentInfo.Id;
                await _context.WordsAssessments.AddAsync(entity);

                var wordList = new List<EnglishWord>();

                if (model.Source == "Library")
                {
                    var userIds = _context.Users.Select(u => u.Id).ToList();

                    var query = _context.EnglishWords.Where(w => w.Book == model.Scope);
                    if (model.Method == "random")
                    {
                        // Ensure non-nullable results to avoid CS8619
                        wordList = query
                            .Where(w => w != null)
                            .OrderBy(w => EF.Functions.Random())
                            .Take(model.WordCount)
                            .Select(w => w!)
                            .ToList();
                    }
                    else
                    {
                        wordList = query
                            .Where(w => w != null)
                            .OrderBy(w => w.Word)
                            .Skip(model.StartIndex - 1)
                            .Take(model.WordCount)
                            .Select(w => w!)
                            .ToList();
                    }

                    foreach (var userId in userIds)
                    {
                        wordList.ForEach(w =>
                        {
                            _context.WordsAssessmentDetails.Add(new WordsAssessmentDetail
                            {
                                Id = Guid.NewGuid(),
                                UserId = userId,
                                WordsAssessementId = model.Id,
                                WordId = w.Id,
                                Word = w.Word,
                                Meaning = w.Meaning
                            });
                        });
                    }
                }
                else if (model.Source == "Unfamiliar")
                {
                    var userIds = model.AssessmentInfo.ForUserIds?.Split(';').ToList();
                    if (userIds != null)
                    {
                        foreach (var forUserId in userIds)
                        {
                            var query = _context.UserEnglishWordRatings
                                .Include(r => r.EnglishWord) // Eager load the Word details
                                .Where(r => r.UserId == forUserId && r.Rating <= 3)
                                .Select(r => r.EnglishWord);

                            if (model.Method == "random")
                            {
                                wordList = query
                                    .Where(w => w != null)
                                    .OrderBy(w => EF.Functions.Random())
                                    .Take(model.WordCount)
                                    .Select(w => w!)
                                    .ToList();
                            }
                            else
                            {
                                wordList = query
                                    .Where(w => w != null)
                                    .OrderBy(w => w.Word)
                                    .Skip(model.StartIndex - 1)
                                    .Take(model.WordCount)
                                    .Select(w => w!)
                                    .ToList();
                            }
                            wordList.ForEach(w =>
                            {
                                _context.WordsAssessmentDetails.Add(new WordsAssessmentDetail
                                {
                                    Id = Guid.NewGuid(),
                                    UserId = forUserId,
                                    WordsAssessementId = model.Id,
                                    WordId = w.Id,
                                    Word = w.Word,
                                    Meaning = w.Meaning
                                });
                            });
                        }
                    }
                }

                await _context.SaveChangesAsync();

                // (Existing code likely continues here to add details, etc.)
                // Return a successful response (ensure method always returns a ResponseModel)
                var success = new ResponseModel<Guid?>()
                {
                    Success = true,
                    Data = entity.Id,
                    Code = "ASSESSMENT-ADDED",
                    Message = "Assessment created successfully."
                };
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                var failed = new ResponseModel<Guid?>()
                {
                    Success = false,
                    Data = null,
                    Code = "RATING-FAILED",
                    Message = "Failed to rate the word."
                };
                return failed;
            }
        }

        public Task<ResponseModel<List<WordsAssessmentModel>>?> GetWordsAssessments()
        {
            var list = _mapper.ProjectTo<WordsAssessmentModel>(_context.WordsAssessments
                        .OrderBy(e => e.AssessmentInfo.Name)).ToList();

            var result = new ResponseModel<List<WordsAssessmentModel>>()
            {
                Success = true,
                Data = list,
                Code = "DATA-LOADED",
                Message = "Data loaded successfully."
            };

            return Task.FromResult<ResponseModel<List<WordsAssessmentModel>>?>(result);
        }
    }
}
