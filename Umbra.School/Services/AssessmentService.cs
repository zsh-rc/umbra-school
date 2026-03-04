using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Umbra.School.Data;
using Umbra.School.Data.Assessment;
using Umbra.School.Data.Blog;
using Umbra.School.Data.English;
using Umbra.School.Migrations;
using Umbra.School.Models;
using Umbra.School.Models.Account;
using Umbra.School.Models.Assessment;
using Umbra.School.Shared;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<ResponseModel<bool>> AddWordsAssessment(WordsAssessmentModel model)
        {
            try
            {
                if (model.Repeatable)
                {
                    await CreateWordsAssessmentsForMultipleDays(model, model.RepeatTimes);
                }
                else
                {
                    await CreateWordsAssessment(model);
                }

                var success = new ResponseModel<bool>()
                {
                    Success = true,
                    Data = true,
                    Code = "ASSESSMENT-ADDED",
                    Message = "Assessment created successfully."
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
                    Code = "RATING-FAILED",
                    Message = "Failed to rate the word."
                };
                return failed;
            }
        }
        private async Task CreateWordsAssessmentsForMultipleDays(WordsAssessmentModel model, int days)
        {
            try
            {
                if (model.AssessmentInfo.ForUserIds == null) return;
                // Get total words count based on the source and scope
                var userIds = model.AssessmentInfo.ForUserIds.Split(';').ToList();
                var userWords = new Dictionary<string, List<EnglishWord>>();
                if (model.Source == "Library")
                {
                    var query = _context.EnglishWords.Where(w => w.Book == model.Scope);
                    // Must order by "Sort" field.
                    var wordList = query
                        .Where(w => w != null)
                        .OrderBy(w => w.Sort)
                        .Skip(model.StartIndex - 1)
                        .Select(w => w!)
                        .ToList();
                    var totalWords = wordList.Count;
                    if (totalWords > 0)
                    {
                        userIds.ForEach(userId =>
                        {
                            userWords[userId] = wordList;
                        });
                    }
                }
                else if (model.Source == "Unfamiliar")
                {
                    userIds.ForEach(userId =>
                    {
                        var wordList = _context.UserEnglishWordRatings
                            .Include(r => r.EnglishWord)
                            .Where(r => r.UserId == userId && r.Rating >=1 && r.Rating <= 3)
                            .OrderBy(r => r.EnglishWord.Word)
                            .Skip(model.StartIndex - 1)
                            .Select(r => r.EnglishWord)
                            .ToList();
                        var totalWords = wordList.Count;
                        if (totalWords > 0)
                            userWords[userId] = wordList;
                    });
                }

                // Re-define WordsAssessmentModel by user.
                foreach (var item in userWords)
                {
                    var userId = item.Key;
                    var wordList = item.Value;
                    var wordsCount = item.Value.Count();

                    // Seperate wordsCount by days
                    var countPerDay = model.WordCount;
                    for (int i = 0; i < days; i++)
                    {
                        // 22
                        // 0 - 9, 10 | 10 - 19, 10 | 20 - 21, 2
                        // 30
                        // 0 - 9, 10 | 10 - 19, 10 | 20 - 29, 10
                        int startIndex = i * countPerDay;
                        int endIndex = startIndex + countPerDay - 1;
                        if (endIndex > wordsCount - 1) countPerDay = wordsCount - startIndex;
                        if (startIndex >= wordsCount) break;
                        // The start and end are used for real index in the whole database.
                        // They are used in WordsAssessments and AssessmentInfoName
                        int start = model.StartIndex + i * countPerDay;
                        int end = start + countPerDay - 1;

                        // Add WordsAssessmentInfo
                        var wordAssessment = _mapper.Map<WordsAssessment>(model);
                        var assessmentInfo = wordAssessment.AssessmentInfo;
                        wordAssessment.AssessmentInfo = null;
                        if (assessmentInfo == null) throw new Exception("No assessment information found.");
                        assessmentInfo.Id = Guid.NewGuid();
                        assessmentInfo.Name = $"{model.AssessmentInfoName} (DAY{i.ToString("D2")}：{start}-{end})";
                        var summary = $"{Dictionaries.WordsAssessmentScopes[model.Scope]} » {Dictionaries.WordsAssessmentSources[model.Source]}（{model.WordCount}以内，{Dictionaries.WordsAssessmentMethods[model.Method]}{(model.Method == "order" ? $"，从{start}开始" : "")}）";
                        assessmentInfo.ShortSummary = summary;
                        await _context.AssessmentInfos.AddAsync(assessmentInfo);
                        // Add Words Assessment
                        wordAssessment.Id = Guid.NewGuid(); // regenerate id for each day
                        wordAssessment.AssessmentInfoId = assessmentInfo.Id;
                        wordAssessment.StartIndex = start;
                        wordAssessment.WordCount = countPerDay;
                        await _context.WordsAssessments.AddAsync(wordAssessment);

                        // Must order by "Sort" field.
                        var assessmentWordList = wordList
                            .Where(w => w != null)
                            .OrderBy(w => w.Sort)
                            .Skip(startIndex)
                            .Take(countPerDay)
                            .Select(w => w!)
                            .ToList();
                        assessmentWordList.ForEach(w =>
                        {
                            _context.WordsAssessmentDetails.Add(new WordsAssessmentDetail
                            {
                                Id = Guid.NewGuid(),
                                UserId = userId,
                                WordsAssessementId = wordAssessment.Id,
                                WordId = w.Id,
                                Word = w.Word,
                                Meaning = w.Meaning
                            });
                        });
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        private async Task CreateWordsAssessment(WordsAssessmentModel model)
        {
            try
            {
                var entity = _mapper.Map<WordsAssessment>(model);
                var assessmentInfo = entity.AssessmentInfo;
                entity.AssessmentInfo = null;
                if (assessmentInfo == null) throw new Exception("No assessment information found.");
                assessmentInfo.Id = Guid.NewGuid();
                await _context.AssessmentInfos.AddAsync(assessmentInfo);
                entity.AssessmentInfoId = assessmentInfo.Id;
                await _context.WordsAssessments.AddAsync(entity);

                var userIds = model.AssessmentInfo.ForUserIds?.Split(';').ToList();
                var wordList = new List<EnglishWord>();

                if (model.Source == "Library")
                {
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
                        // Must order by "Sort" field.
                        wordList = query
                            .Where(w => w != null)
                            .OrderBy(w => w.Sort)
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
                    if (userIds != null)
                    {
                        foreach (var forUserId in userIds)
                        {
                            var query = _context.UserEnglishWordRatings
                                .Include(r => r.EnglishWord) // Eager load the Word details
                                .Where(r => r.UserId == forUserId && r.Rating >= 1 && r.Rating <= 3)
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
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

        public Task<ResponseModel<List<AssessmentResultModel>>> GetAssessmentResults(string category)
        {
            var query = _context.AssessmentResults
                .Where(e => e.AssessementInfo.Category == category)
                .Include(e => e.AssessementInfo)
                .Include(e => e.ApplicationUser)
                .OrderByDescending(e => e.Date);
            var list = _mapper.ProjectTo<AssessmentResultModel>(query).ToList();
            var result = new ResponseModel<List<AssessmentResultModel>>()
            {
                Success = true,
                Data = list,
                Code = "DATA-LOADED",
                Message = "Data loaded successfully."
            };
            return Task.FromResult(result);
        }

        public Task<ResponseModel<List<WordsAssessmentDetailModel>>> GetWordsAssessmentDetails(string userId, Guid assessmentInfoId)
        {
            var query = _context.WordsAssessmentDetails
                .Where(d => d.WordsAssessment.AssessmentInfoId == assessmentInfoId && d.UserId == userId)
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

            list.ForEach(assessment =>
            {
                var assessmentResults = _mapper.ProjectTo<AssessmentResultModel>(_context.AssessmentResults.Where(e => e.UserId == userId && e.AssessmentInfoId == assessment.AssessmentInfoId));
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

        public async Task<ResponseModel<bool>> RectifyAssessmentDetailRecord(Guid id, bool correct)
        {
            try
            {
                var detailRecord = await _context.WordsAssessmentDetails.FirstOrDefaultAsync(e => e.Id == id);
                if (detailRecord == null)
                {
                    throw new Exception("No record found.");
                }
                detailRecord.Correct = correct;
                await _context.SaveChangesAsync();
                var success = new ResponseModel<bool>()
                {
                    Success = true,
                    Data = true,
                    Code = "RECTIFY-ASSESSMENT-RECORD-SUCCESS",
                    Message = "Rectified successfully."
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
                    Code = "RECTIFY-ASSESSMENT-RECORD-FAILED",
                    Message = "Rectified failed."
                };
                return failed;
            }
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
                        AssessementInfo = null,
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

        public async Task<ResponseModel<bool>> SubmitReviewResult(AssessmentResultModel review)
        {
            try
            {
                var reviewRecord = await _context.AssessmentResults.FirstOrDefaultAsync(e => e.Id == review.Id);
                if (reviewRecord == null)
                {
                    throw new Exception("No record found.");
                }

                reviewRecord.Score = review.Score;
                reviewRecord.Feedback = review.Feedback;
                reviewRecord.ReviewDate = DateTime.UtcNow;
                reviewRecord.ReviewedBy = review.ReviewedBy;
                reviewRecord.Status = review.Status;
                await _context.SaveChangesAsync();
                var success = new ResponseModel<bool>()
                {
                    Success = true,
                    Data = true,
                    Code = "REVIEW-SUCCESS",
                    Message = "Review successfully."
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
                    Code = "REVIEW-FAILED",
                    Message = "Review failed."
                };
                return failed;
            }
        }
    }
}
