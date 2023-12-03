using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Polly.Data
{
    public class Role : IdentityRole<long>
    {
        public Role() { }
    }

    public class User : IdentityUser<long>
    {
        public User()
        {
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
