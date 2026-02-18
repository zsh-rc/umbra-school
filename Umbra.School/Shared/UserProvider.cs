using Microsoft.AspNetCore.Components.Authorization;
using Umbra.School.Models;

namespace Umbra.School.Shared
{
    public class UserProvider
    {
        private readonly AuthenticationStateProvider _authStateProvider;

        public UserProvider(AuthenticationStateProvider authStateProvider)
        {
            _authStateProvider = authStateProvider;
        }

        public async Task<UserSessionModel> GetUserAsync()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated != true)
                return new UserSessionModel();

            return new UserSessionModel
            {
                UserId = user.FindFirst("CustomUserId")?.Value ?? "",
                Username = user.Identity.Name ?? "",
                Email = user.FindFirst("EmailAddress")?.Value ?? "",
                IsAdmin = user.IsInRole("Admin")
            };
        }
    }

}
