using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using System.Net.Sockets;
using Umbra.School.Data;
using Umbra.School.Models;
using Umbra.School.Models.Account;
using Umbra.School.Models.English;
using Umbra.School.Shared;

namespace Umbra.School.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountService(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ResponseModel<bool>> AddUserRoles(string userId, List<ApplicationRoleModel> roles)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) throw new Exception("User is not found.");
                var result = await _userManager.AddToRolesAsync(user, roles.Select(r => r.Name).ToList());
                var updatedRoles = await _userManager.GetRolesAsync(user);
                return new ResponseModel<bool> { Success = true, Data = true, Code = "USER_ROLES_ADDED", Message = $"User role(s) is added." };
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool> { Success = false, Data = false, Code = "UNKNOWN_ERROR", Message = $"Unknown error! ({ex})" };
            }
        }

        public async Task<ResponseModel<bool>> ConfirmEmail(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) throw new Exception("User is not found.");
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return new ResponseModel<bool> { Success = true, Data = true, Code = "CONFIRM_EMAIL_SUCCESS", Message = $"Email is confirmed." };
                }
                else
                {
                    var errorMessage = string.Empty;
                    var errors = result.Errors;
                    foreach (var error in errors)
                    {
                        errorMessage += $"{error.Description} ({error.Code}). ";
                    }
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool> { Success = false, Data = false, Code = "UNKNOWN_ERROR", Message = $"Unknown error! ({ex})" };
            }
        }

        public Task<List<ApplicationUserModel>> GetAllApplicationUsers()
        {
            var list = _mapper.ProjectTo<ApplicationUserModel>(_context.Users
                        .OrderBy(e => e.UserName)).ToList();

            return Task.FromResult(list);
        }

        public async Task<List<ApplicationRoleModel>> GetUserMissingRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("User is not found.");
            var userRoleNames = await _userManager.GetRolesAsync(user);
            var roles = _roleManager.Roles.ToList();
            var list = new List<ApplicationRoleModel>();
            // Find missing roles.
            foreach (var role in roles)
            {
                if (!userRoleNames.Contains(role.Name))
                {
                    list.Add(new ApplicationRoleModel()
                    {
                        Id = role!.Id,
                        Name = role!.Name,
                        Enabled = role.Enabled
                    });
                }
            }
            return list;
        }

        public async Task<List<ApplicationRoleModel>> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("User is not found.");
            var roleNames = await _userManager.GetRolesAsync(user);
            var list = new List<ApplicationRoleModel>();
            if (roleNames != null)
            {
                foreach (var name in roleNames)
                {
                    var role = await _roleManager.FindByNameAsync(name);
                    list.Add(new ApplicationRoleModel()
                    {
                        Id = role!.Id,
                        Name = role!.Name,
                        Enabled = role.Enabled
                    });
                }
            }
            return list;
        }

        public async Task<ResponseModel<bool>> LockUser(string userId, bool locked)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) throw new Exception("User is not found.");
                var result = await _userManager.SetLockoutEndDateAsync(user, locked ? DateTimeOffset.Now.AddMinutes(5) : null);
                if (!locked) result = await _userManager.ResetAccessFailedCountAsync(user);
                if (result.Succeeded)
                {
                    return new ResponseModel<bool> { Success = true, Data = true, Code = locked ? "LOCK_USER_SUCCESS" : "UNLOCK_USER_SUCCESS", Message = $"{(locked ? "Lock" : "Unlock")} user is confirmed." };
                }
                else
                {
                    var errorMessage = string.Empty;
                    var errors = result.Errors;
                    foreach (var error in errors)
                    {
                        errorMessage += $"{error.Description} ({error.Code}). ";
                    }
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool> { Success = false, Data = false, Code = "UNKNOWN_ERROR", Message = $"Unknown error! ({ex})" };
            }
        }

        public async Task<ResponseModel<bool>> RemoveUserRole(string userId, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) throw new Exception("User is not found.");
                var result = await _userManager.RemoveFromRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    return new ResponseModel<bool>
                    {
                        Success = true,
                        Data = true,
                        Code = "USER_ROLE_REMOVED",
                        Message = $"User's role {roleName} is removed"
                    };
                }
                else
                {
                    var errorMessage = string.Empty;
                    var errors = result.Errors;
                    foreach (var error in errors)
                    {
                        errorMessage += $"{error.Description} ({error.Code}). ";
                    }
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool> { Success = false, Data = false, Code = "UNKNOWN_ERROR", Message = $"Unknown error! ({ex})" };
            }
        }

        public async Task<ResponseModel<string>> ResetPassword(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) throw new Exception("User is not found.");
                string password = UtilityHelper.AutoPassword();
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, password);
                if (result.Succeeded)
                {
                    return new ResponseModel<string> { Success = true, Data = password, Code = "PASSWORD_RESET_SUCCESS", Message = $"Password was reset successfully." };
                }
                else
                {
                    var errorMessage = string.Empty;
                    var errors = result.Errors;
                    foreach (var error in errors)
                    {
                        errorMessage += $"{error.Description} ({error.Code}). ";
                    }
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel<string> { Success = false, Data = null, Code = "UNKNOWN_ERROR", Message = $"Unknown error! ({ex})" };
            }
        }
    }
}
