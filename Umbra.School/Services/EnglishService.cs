using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Umbra.School.Data;
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

        public Task<ResponseModel<List<EnglishWordModel>>?> GetEnglishWordsByAlphabet(string alphabet)
        {
            var list = _mapper.ProjectTo<EnglishWordModel>(_context.EnglishWords
                        .Where(e => e.Word.StartsWith(alphabet))
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

        public Task<ResponseModel<bool>?> UpdateEnglishWord(EnglishWordModel word)
        {
            throw new NotImplementedException();
        }
    }
}
