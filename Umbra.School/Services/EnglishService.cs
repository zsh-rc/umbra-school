using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
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

        public async Task<ResponseModel<WordListModel>> GetEnglishWordsWithRating(string username, string? book, string? alphabet)
        {
            var userId = _context.Users.First(u => u.NormalizedUserName == username.ToUpper()).Id;
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

            // 5. Project directly to the model (AutoMapper handles the LEFT JOIN to UserRatings)
            // Pass userId as a parameter for the Mapping Profile
            var list = await baseQuery
                .ProjectTo<EnglishWordModel>(_mapper.ConfigurationProvider, new { userId })
                .OrderBy(e => e.Word)
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


        public async Task<ResponseModel<bool>> RatingEnglishWord(string username, Guid wordId, int rating)
        {
            try
            {
                var normalizedUsername = username?.ToUpperInvariant();
                var user = _context.Users.FirstOrDefault(u => u.NormalizedUserName == normalizedUsername);
                if (user == null)
                {
                    var notFoundUser = new ResponseModel<bool>()
                    {
                        Success = false,
                        Data = false,
                        Code = "USER-NOT-FOUND",
                        Message = "User not found."
                    };
                    return notFoundUser;
                }

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

                var findRating = _context.UserEnglishWordRatings.FirstOrDefault(r => r.UserId == user.Id && r.WordId == wordId);
                if (findRating == null)
                {
                    _context.Add(new UserEnglishWordRating()
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
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
