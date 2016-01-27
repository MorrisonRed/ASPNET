using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using AppCore;
using System.Data.Entity;
using ASPNET.Models;


namespace ASPNET
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //load application configuration 
            var appConfiguration = ApplicationConfiguration.Instance;

            //this throw and error
            //var config = new AppCore.ApplicationConfiguration();
            //ApplicationConfiguration.Save(config);


            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //implementation of Customer Controller Factory
            var factory = new CustomControllerFactory();
            ControllerBuilder.Current.SetControllerFactory(factory);

            //Database.SetInitializer<ApplicationDbContext>(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
        }
    }
}
