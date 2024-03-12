using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Window.Application.StticTools;
using Window.Domain.Entities.Account;

namespace Window.Application.Extensions
{
    public static class UsersUtilities
    {
        public static string GetUserAvatar(this User user)
        {
            return !string.IsNullOrEmpty(user.Avatar) ? FilePaths.UserAvatarPath + user.Avatar : FilePaths.DefaultUserAvatar;
        }

        public static string GetUserAvatar(this string userAvatar)
        {
            return !string.IsNullOrEmpty(userAvatar) ? FilePaths.UserAvatarPath + userAvatar : FilePaths.DefaultUserAvatar;
        }

        public static ulong GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal != null)
            {
                var data = claimsPrincipal.Claims.SingleOrDefault(s => s.Type == ClaimTypes.NameIdentifier);
                if (data != null) return (ulong)Convert.ToInt32(data?.Value);
                return default(long);
            }

            return default(long);
        }

        public static ulong GetUserId(this IPrincipal principal)
        {
            var user = (ClaimsPrincipal)principal;

            return user.GetUserId();
        }

        public static string GetFullName(this User user)
        {
            return user.Username;
        }
    }
}
