using Umbra.School.Models;
using Umbra.School.Models.Account;
using Umbra.School.Models.Assessment;

namespace Umbra.School.Services
{
    public interface IAccountService
    {
        Task<List<ApplicationUserModel>> GetAllApplicationUsers();
        Task<ResponseModel<string>> ResetPassword(string userId);
        Task<ResponseModel<bool>> ConfirmEmail(string userId);
        Task<ResponseModel<bool>> LockUser(string userId, bool locked);
        Task<List<ApplicationRoleModel>> GetUserRoles(string userId);
        Task<List<ApplicationRoleModel>> GetUserMissingRoles(string userId);
        Task<ResponseModel<bool>> RemoveUserRole(string userId, string roleName);
        Task<ResponseModel<bool>> AddUserRoles(string userId, List<ApplicationRoleModel> roles);
        Task<List<ApplicationRoleModel>> GetAllApplicationRoles();
        Task<ResponseModel<bool>> EnableRole(string roleId, bool enabled);
        Task<List<ApplicationUserModel>> GetRoleUsers(string roleId);
        Task<List<ApplicationUserModel>> GetUsersWithoutTheRole(string roleId);
        Task<ResponseModel<bool>> RemoveRoleUser(string roleId, string userId);
        Task<ResponseModel<bool>> AddRoleUsers(string roleId, List<ApplicationUserModel> users);
        Task<ResponseModel<bool>> CreateRole(ApplicationRoleModel model);
        Task<ResponseModel<bool>> UpdateRole(ApplicationRoleModel model);
        Task<ResponseModel<bool>> DeleteRole(string roleId);

    }
}
