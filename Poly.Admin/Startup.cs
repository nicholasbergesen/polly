using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Poly.Admin.Startup))]
namespace Poly.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
