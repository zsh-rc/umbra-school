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
using Umbra.School.Shared;

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

        public async Task<ResponseModel<bool>> EndAssessment(Guid assessmentInfoId, string userId)
        {
            try
            {
                var assessmentResult = await _context.AssessmentResults.FirstOrDefaultAsync(e => e.AssessmentInfoId == assessmentInfoId && e.UserId == userId);
                if (assessmentResult == null)
                {
                    throw new Exception("No assessment information found.");
                }
                assessmentResult.EndTime = DateTime.UtcNow;
                assessmentResult.Duration = (int)Math.Round(assessmentResult.EndTime.Value.Subtract(assessmentResult.StartTime.Value).TotalMinutes);
                assessmentResult.Status = AssessmentStatus.Submitted.GetDescription();
                await _context.SaveChangesAsync();
                var success = new ResponseModel<bool>()
                {
                    Success = true,
                    Data = true,
                    Code = "ASSESSMENT-STARTED",
                    Message = "Assessment was started."
                };
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                var failed = new ResponseModel<bool>()
                {
                    Success = false,
                    Data = false,
                    Code = "START-ASSESSMENT-FAILED",
                    Message = ex.Message
                };
                return failed;
            }
        }

        public Task<ResponseModel<List<WordsAssessmentDetailModel>>> GetWordsAssessmentDetail(string userId, Guid wordAssessmentId)
        {
            var query = _context.WordsAssessmentDetails
                .Where(d => d.WordsAssessementId == wordAssessmentId && d.UserId == userId)
                .OrderBy(d => d.Word);
            var list = _mapper.ProjectTo<WordsAssessmentDetailModel>(query).ToList();
            var result = new ResponseModel<List<WordsAssessmentDetailModel>>()
            {
                Success = true,
                Data = list,
                Code = "DATA-LOADED",
                Message = "Data loaded successfully."
            };
            return Task.FromResult(result);
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

        public Task<ResponseModel<List<WordsAssessmentModel>>?> GetWordsAssessments(string userId)
        {
            var list = _mapper.ProjectTo<WordsAssessmentModel>(_context.WordsAssessments
                        .Where(e => e.AssessmentInfo.ForUserIds == "*" || e.AssessmentInfo.ForUserIds.Contains(userId))
                        .OrderBy(e => e.AssessmentInfo.Name)).ToList();

            list.ForEach(assessment => {
                var assessmentResults = _mapper.ProjectTo< AssessmentResultModel>(_context.AssessmentResults.Where(e => e.UserId == userId && e.AssessmentInfoId == assessment.AssessmentInfoId));
                if (assessmentResults.Count() > 0)
                {
                    assessment.UserId = userId;
                    assessment.AssessmentResult = assessmentResults.First();
                }                
            });

            var result = new ResponseModel<List<WordsAssessmentModel>>()
            {
                Success = true,
                Data = list,
                Code = "DATA-LOADED",
                Message = "Data loaded successfully."
            };

            return Task.FromResult<ResponseModel<List<WordsAssessmentModel>>?>(result);
        }

        public async Task<ResponseModel<bool>> StartAssessment(Guid assessmentInfoId, string userId)
        {
            try
            {
                var assessmentResult = await _context.AssessmentResults.FirstOrDefaultAsync(e => e.AssessmentInfoId == assessmentInfoId && e.UserId == userId);
                if (assessmentResult == null)
                {
                    assessmentResult = new AssessmentResult()
                    {
                        Id = Guid.NewGuid(),
                        AssessmentInfoId = assessmentInfoId,
                        UserId = userId
                    };
                    await _context.AssessmentResults.AddAsync(assessmentResult);
                }                
                assessmentResult.Date = DateTime.Today;
                assessmentResult.StartTime = DateTime.UtcNow;
                assessmentResult.Status = AssessmentStatus.InProgress.GetDescription();
                await _context.SaveChangesAsync();
                var success = new ResponseModel<bool>()
                {
                    Success = true,
                    Data = true,
                    Code = "ASSESSMENT-STARTED",
                    Message = "Assessment was started."
                };
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                var failed = new ResponseModel<bool>()
                {
                    Success = false,
                    Data = false,
                    Code = "START-ASSESSMENT-FAILED",
                    Message = ex.Message
                };
                return failed;
            }
        }

        public async Task<ResponseModel<bool>> SubmitAssessmentAnswers(List<WordsAssessmentDetailModel> answers)
        {
            if (answers.Count == 0)
            {
                var failed = new ResponseModel<bool>()
                {
                    Success = false,
                    Data = false,
                    Code = "EMPTY-ANSWERS",
                    Message = "No answers provided"
                };
                return failed;
            }

            var assessmentInfoId = answers.First().WordsAssessment.AssessmentInfoId;
            var userId = answers.First().UserId;

            try
            {
                foreach (var answer in answers)
                {
                    var find = await _context.FindAsync<WordsAssessmentDetail>(answer.Id);
                    if (find != null)
                    {
                        find.Answer = answer.Answer;
                        find.Correct = answer.Word.Equals(find.Answer, StringComparison.OrdinalIgnoreCase);
                    }
                }
                // update assessment result.
                var assessmentResult = await _context.AssessmentResults.FirstOrDefaultAsync(e => e.AssessmentInfoId == assessmentInfoId && e.UserId == userId);
                if (assessmentResult == null)
                {
                    throw new Exception("No assessment information found.");
                }
                assessmentResult.EndTime = DateTime.UtcNow;
                assessmentResult.Duration = (int)Math.Round(assessmentResult.EndTime.Value.Subtract(assessmentResult.StartTime.Value).TotalMinutes);
                assessmentResult.Status = AssessmentStatus.Submitted.GetDescription();
                await _context.SaveChangesAsync();
                var success = new ResponseModel<bool>()
                {
                    Success = true,
                    Data = true,
                    Code = "ANSWERS-SUBMITTED",
                    Message = "Answers are submitted successfully."
                };
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                var failed = new ResponseModel<bool>()
                {
                    Success = false,
                    Data = false,
                    Code = "SUBMIT-ANSWERS-FAILED",
                    Message = "Submit answers failed"
                };
                return failed;
            }
        }
    }
}
