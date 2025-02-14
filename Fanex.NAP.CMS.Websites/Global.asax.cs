// --------------------------------------------------------------------------
// <copyright file="Global.asax.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.3">Register Fanex.Data.</change>
// </history>

namespace Fanex.NAP.CMS.Websites
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Fanex.Caching;
    using Fanex.Data;
    using Fanex.Data.SqlServer;
    using Logging.Extensions.Web;
    using Nap.CMS.Infrastructure.Web;
    using ONEbook.UM;
    using ONEbook.UM.AspNetMVC.Filters;
    using ONEbook.UM.Configurations;
    using ONEbook.UM.Helpers;
    using ONEbook.UM.NETFramework;
    using ONEbook.UM.Services;
    using ONEbook.UM.Transport;
    using RemotingRef;

    /// <summary>
    /// MVC application configuration.
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
        protected void Application_Start()
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
        {
            LoggingConfig.Init(
                  new[] { "password", "OldUserPwd", "UserPwd", "ConfirmUserPwd" });

            var umConfig = new UMClientConfiguration().Initialize();
            UMClientManager
                .UseConfig(umConfig)
                .UseCacheService(new UMCacheService(new CacheService()))
                .UseRestClient(new UMRestClient(umConfig))
                .UseHttpContext(new HttpContextHelper());
            GlobalFilters.Filters.Add(new CheckUMFilterAttribute(
                new MaintenanceService(UMClientManager.RestClient, UMClientManager.CacheService, UMClientManager.UMClientConfigs, UMClientManager.HttpContext), null));

            DbSettingProviderManager.StartNewSession()
                .WithSqlServer()
                .UseDefaultDbSettingProvider(ConfigurationManager.AppSettings["StoredProcedurePath"], true)
                .UseDefaultConnectionStringProvider()
                .Run();
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            RefDataTypeContainer
                .ConnectTo(new Uri(ConfigurationManager.AppSettings["RemoteRefURL"]))
                .FromApplication("Fanex.NAP.CMS")
                .SupportOnlyDefaultLanguage();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            ExceptionHandler.Handle(Server.GetLastError());
        }

        protected void Application_EndRequest()
        {
            var context = new HttpContextWrapper(Context);
            // If we're an ajax request, and doing a 302, then we actually need to do a 403
            if (Context.Response.StatusCode == (int)HttpStatusCode.Redirect && context.Request.IsAjaxRequest())
            {
                Context.Response.Clear();
                Context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }
    }
}
