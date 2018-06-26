using SalesAdminPortal.Migrations;
using SalesAdminPortal.Models;
using Swashbuckle.Application;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SalesAdminPortal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //GlobalConfiguration.Configuration
            //          .EnableSwagger(c => c.SingleApiVersion("v1", "A title for your API"))
            //          .EnableSwaggerUi();
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
            var db = new ApplicationDbContext();
            db.Database.Initialize(true);
        }
    }
}
