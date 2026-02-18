using AutoMapper;
using Umbra.School.Data;
using Umbra.School.Models;
using Umbra.School.Models.Account;
using Umbra.School.Models.English;

namespace Umbra.School.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AccountService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<ResponseModel<List<ApplicationUserModel>>?> GetAllApplicationUsers()
        {
            var list = _mapper.ProjectTo<ApplicationUserModel>(_context.Users
                        .OrderBy(e => e.UserName)).ToList();

            var result = new ResponseModel<List<ApplicationUserModel>>()
            {
                Success = true,
                Data = list,
                Code = "DATA-LOADED",
                Message = "Data loaded successfully."
            };

            return Task.FromResult<ResponseModel<List<ApplicationUserModel>>?>(result);
        }
    }
}
