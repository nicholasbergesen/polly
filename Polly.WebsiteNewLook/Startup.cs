using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Polly.WebsiteNewLook.Startup))]
namespace Polly.WebsiteNewLook
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
