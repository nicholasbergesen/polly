using Polly.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polly.Website.Models
{
    public class AdminUserRoleView
    {
        public AdminUserRoleView()
        {
            
        }

        public AdminUserRoleView(User user, IList<string> roles)
        {
            Id = user.Id;
            Email = user.Email;
            IsEnabled = user.IsEnabled;
            EmailConfirmed = user.EmailConfirmed;
            if(roles != null)
                Roles = string.Join(",", roles);
        }

        public long Id { get; set; }
        public string Email { get; set; }
        public bool IsEnabled { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Roles { get; set; }
    }
}