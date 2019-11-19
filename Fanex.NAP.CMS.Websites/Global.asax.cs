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
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Data.Configuration;
    using Logging.Extensions.Database;
    using Logging.Extensions.Web;
    using Nap.CMS.Infrastructure.Web;
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
                  new[] { "password", "OldUserPwd", "UserPwd", "ConfirmUserPwd" },
                  getDbExceptionInfo: DatabaseInfoGetters.GetDbExceptionInfo);

            DbSettingProviderManager.Current.Start();
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
    }
}
