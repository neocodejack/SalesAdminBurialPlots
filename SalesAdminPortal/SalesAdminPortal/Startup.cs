﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SalesAdminPortal.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
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
