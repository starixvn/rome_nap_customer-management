// --------------------------------------------------------------------------
// <copyright file="ErrorController.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.3">Created</change>
// </history>

namespace Fanex.NAP.CMS.Controllers
{
    using System.Web.Mvc;
    using Fanex.NAP.Client.Attributes;

    /// <summary>
    /// Error controller.
    /// </summary>
    [NapSuperAuth]
    public class ErrorController : BaseController
    {
        /// <summary>
        /// Redirect to default error page.
        /// </summary>
        /// <returns>Access Denied view.</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Redirect to access denied page.
        /// </summary>
        /// <returns>Access Denied view.</returns>
        public ActionResult AccessDenied()
        {
            return View();
        }

        /// <summary>
        /// Redirect to not found page.
        /// </summary>
        /// <returns>Access Denied view.</returns>
        public ActionResult NotFound()
        {
            return View();
        }
    }
}