using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Polly.Website.Startup))]
namespace Polly.Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
