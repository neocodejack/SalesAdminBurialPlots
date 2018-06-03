using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SalesAdminPortal.Startup))]
namespace SalesAdminPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
