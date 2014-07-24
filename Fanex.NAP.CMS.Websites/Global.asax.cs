// --------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.3">Register Fanex.Data.</change>
// </history>

namespace Fanex.NAP.CMS.Websites
{
    using System;
    using System.Data.SqlClient;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Fanex.Data.Configuration;
    using Fanex.Logging;

    /// <summary>
    /// MVC application configuration.
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Sets all needed configuration when the application starts.
        /// </summary>
        protected void Application_Start()
        {
            DbSettingProviderManager.Current.Start();
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Handles the Error event of the Application.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError().GetBaseException();
            SqlException sqlEx = ex as SqlException;
            string exceptionFormat = "{0}\r\nType: {1}" +
                                     "\r\nSource: {2}" +
                                     "\r\nDomain: {3}" +
                                     "\r\nReferrer: {4}" +
                                     "\r\nIP: {5}" +
                                     "\r\nUserAgent: {6}" +
                                     "\r\nForm Variable: {7}" +
                                     "\r\nQueryString: {8}" +
                                     "\r\nTargetSite: {9}" +
                                     "\r\nStackTrace: {10}";

            if (!Request.IsLocal)
            {
                string message = string.Format(
                                            exceptionFormat,
                                            ex.Message,
                                            ex.GetType().ToString(),
                                            ex.Source,
                                            HttpContext.Current.Request.Headers["Host"],
                                            HttpContext.Current.Request.UrlReferrer,
                                            HttpContext.Current.Request.UserHostAddress,
                                            HttpContext.Current.Request.UserAgent,
                                            Request.Form.ToString(),
                                            Request.QueryString.ToString(),
                                            ex.TargetSite,
                                            ex.StackTrace);

                // Get additional exception data for SqlException.
                if (sqlEx != null)
                {
                    message += string.Format(
                                          "\r\nProcedure: {0}\r\nLine Number: {1}\r\nServer: {2}",
                                          sqlEx.Procedure,
                                          sqlEx.LineNumber,
                                          sqlEx.Server);
                }

                LogService.Error(message);
            }
        }
    }
}