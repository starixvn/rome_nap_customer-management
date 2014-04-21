// --------------------------------------------------------------------------
// <copyright file="ConfigureSMATOByMatchController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.16">Created</change>
// </history>

namespace Fanex.NAP.CMS.Controllers
{
    using System.Web.Mvc;
    using Fanex.NAP.CMS.Services.Implements;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Configure turnover by event controller.
    /// </summary>
    public class ConfigureSMATOByMatchController : BaseController
    {
        private readonly IConfigureSMATOByMatchService _configureSMATOByMatchService;

        public ConfigureSMATOByMatchController() :
            this(null)
        {
        }

        public ConfigureSMATOByMatchController(IConfigureSMATOByMatchService configureSMATOByMatchService)
        {
            this._configureSMATOByMatchService = configureSMATOByMatchService ?? new ConfigureSMATOByMatchService();
            ViewBag.HasEditAccessRight = this.HasRight(CMSFunctionKeys.ConfigureSMATOByMatch, Constant.Policy.Host);
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.ConfigureSMATOByMatch, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View();
        }
    }
}