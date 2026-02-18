using Umbra.School.Models;
using Umbra.School.Models.Account;
using Umbra.School.Models.Assessment;

namespace Umbra.School.Services
{
    public interface IAccountService
    {
        Task<ResponseModel<List<ApplicationUserModel>>?> GetAllApplicationUsers();
    }
}
