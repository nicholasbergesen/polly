using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Polly.Website
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            //https://simpleinjector.readthedocs.io/en/latest/mvcintegration.html
            //https://simpleinjector.readthedocs.io/en/latest/webapiintegration.html
            //var container = new Container();
            //container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            //Domain.RegisterDI.Register(container, Lifestyle.Scoped);
            //Data.RegisterDI.Register(container, Lifestyle.Scoped);
            //container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            //container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            //container.Verify();

            //GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
            //DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            // Here your usual Web API configuration stuff.

            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            //TopTenCache.PopulateTopTenCache().Wait();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }
    }
}
