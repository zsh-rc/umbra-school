using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbra.School.Data;
using Umbra.School.Data.PersonalData;
using Umbra.School.Models;
using Umbra.School.Models.English;

namespace Umbra.School.Services
{
    public class EnglishService : IEnglishService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EnglishService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<ResponseModel<Guid>?> AddEnglishWord(EnglishWordModel word)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<bool>?> DeleteEnglishWord(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<EnglishWordModel>?> GetEnglishWordById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<List<EnglishWordModel>>?> GetEnglishWords()
        {
            var list = _mapper.ProjectTo<EnglishWordModel>(_context.EnglishWords
                        .OrderBy(e => e.Word)).ToList();

            var result = new ResponseModel<List<EnglishWordModel>>()
            {
                Success = true,
                Data = list,
                Code = "DATA-LOADED",
                Message = "Data loaded successfully."
            };

            return Task.FromResult<ResponseModel<List<EnglishWordModel>>?>(result);
        }

        public Task<ResponseModel<WordListModel>> GetEnglishWords(string? book, string? alphabet)
        {
            var totalWordsOfBook = _context.EnglishWords
                        .Where(e => (string.IsNullOrEmpty(book) || e.Book.ToLower() == book.ToLower()))
                        .Count();
            var list = _mapper.ProjectTo<EnglishWordModel>(_context.EnglishWords
                        .Where(e => (string.IsNullOrEmpty(book) || e.Book.ToLower() == book.ToLower())
                            && (string.IsNullOrEmpty(alphabet) || e.Word.StartsWith(alphabet)))
                        .OrderBy(e => e.Word)).ToList();

            var wordListModel = new WordListModel()
            {
                TotalWordsOfBook = totalWordsOfBook,
                EnglishWords = list
            };

            var result = new ResponseModel<WordListModel>()
            {
                Success = true,
                Data = wordListModel,
                Code = "DATA-LOADED",
                Message = "Data loaded successfully."
            };

            return Task.FromResult(result);
        }

        public async Task<ResponseModel<WordListModel>> GetEnglishWordsWithRatings(string userId, string? book, string? alphabet, int? start, int? end)
        {
            // 1. Prepare base query with NoTracking for speed
            var baseQuery = _context.EnglishWords.AsNoTracking();

            // 2. Filter by Book (Case-insensitive)
            if (!string.IsNullOrEmpty(book))
            {
                baseQuery = baseQuery.Where(e => e.Book.ToLower() == book.ToLower());
            }

            // 3. Get Total Count before applying the Alphabet filter
            var totalWordsOfBook = await baseQuery.CountAsync();

            // 4. Apply Alphabet filter for the final list
            if (!string.IsNullOrEmpty(alphabet))
            {
                baseQuery = baseQuery.Where(w => w.Word.StartsWith(alphabet));
            }

            if (start != null || end != null)
            {
                var startIndex = (start == null ? 1 : start.Value);
                var endIndex = (end == null ? totalWordsOfBook : end.Value);
                var count = endIndex - startIndex + 1;
                baseQuery = baseQuery.OrderBy(e => e.Sort).Skip(startIndex - 1).Take(count);
            }

            // Project directly to the model (AutoMapper handles the LEFT JOIN to UserRatings)
            // Pass userId as a parameter for the Mapping Profile
            var list = await baseQuery
                .ProjectTo<EnglishWordModel>(_mapper.ConfigurationProvider, new { userId })
                .OrderBy(e => e.Sort)
                .ToListAsync();

            return new ResponseModel<WordListModel>
            {
                Success = true,
                Data = new WordListModel
                {
                    TotalWordsOfBook = totalWordsOfBook,
                    EnglishWords = list
                },
                Code = "DATA-LOADED",
                Message = "Data loaded successfully."
            };
        }

        public async Task<ResponseModel<WordListModel>> GetUnfamiliarEnglishWords(string userId, string? alphabet, int? start, int? end)
        {
            var baseQuery = _context.EnglishWords.AsNoTracking();

            // Project directly to the model (AutoMapper handles the LEFT JOIN to UserRatings)
            // Pass userId as a parameter for the Mapping Profile
            var list = await baseQuery
                .ProjectTo<EnglishWordModel>(_mapper.ConfigurationProvider, new { userId })
                .Where(e => e.Rating >=1 && e.Rating <= 3)
                .ToListAsync();

            var totalWordsOfBook = list.Count();                      

            if (!string.IsNullOrEmpty(alphabet))
            {
                list = list.Where(w => w.Word.StartsWith(alphabet)).ToList();
            }

            if (start != null || end != null)
            {
                var startIndex = (start == null ? 1 : start.Value);
                var endIndex = (end == null ? totalWordsOfBook : end.Value);
                var count = endIndex - startIndex + 1;
                list = list.OrderBy(e => e.Sort).Skip(startIndex - 1).Take(count).ToList();
            }

            return new ResponseModel<WordListModel>
            {
                Success = true,
                Data = new WordListModel
                {
                    TotalWordsOfBook = totalWordsOfBook,
                    EnglishWords = list
                },
                Code = "DATA-LOADED",
                Message = "Data loaded successfully."
            };
        }

        public async Task<ResponseModel<bool>> RatingEnglishWord(string userId, Guid wordId, int rating)
        {
            try
            {
                var word = _context.EnglishWords.FirstOrDefault(w => w.Id == wordId);
                if (word == null)
                {
                    var notFoundWord = new ResponseModel<bool>()
                    {
                        Success = false,
                        Data = false,
                        Code = "WORD-NOT-FOUND",
                        Message = "Word not found."
                    };
                    return notFoundWord;
                }

                var findRating = _context.UserEnglishWordRatings.FirstOrDefault(r => r.UserId == userId && r.WordId == wordId);
                if (findRating == null)
                {
                    _context.Add(new UserEnglishWordRating()
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId,
                        WordId = wordId,
                        Rating = rating,
                        LastReviewed = DateTime.UtcNow,
                    });
                }
                else
                {
                    findRating.Rating = rating;
                    findRating.LastReviewed = DateTime.UtcNow;
                    _context.Update(findRating);
                }

                await _context.SaveChangesAsync();

                var success = new ResponseModel<bool>()
                {
                    Success = true,
                    Data = true,
                    Code = "RATING-SAVED",
                    Message = "Rating saved successfully."
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

        public Task<ResponseModel<bool>?> UpdateEnglishWord(EnglishWordModel word)
        {
            throw new NotImplementedException();
        }
    }
}
