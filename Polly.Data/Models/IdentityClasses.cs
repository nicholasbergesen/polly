using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Polly.Data
{
    public class Role : IdentityRole<long, UserRole>
    {
        public Role() { }
    }

    public class User : IdentityUser<long, UserLogin, UserRole, UserClaim>
    {
        public User() 
        {
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, long> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public bool IsEnabled { get; set; }
    }


    public class UserRole : IdentityUserRole<long>
    {
        public UserRole() { }
    }

    public class UserLogin : IdentityUserLogin<long>
    {
        public UserLogin() { }
    }

    public class UserClaim : IdentityUserClaim<long>
    {
        public UserClaim() { }
    }
}
