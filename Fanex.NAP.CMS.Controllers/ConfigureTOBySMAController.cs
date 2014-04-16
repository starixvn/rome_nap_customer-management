// --------------------------------------------------------------------------
// <copyright file="ConfigureTOBySMAController.cs" company="Nexcel Solutions Vietnam">
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
    /// Configure turnover by SMA controller.
    /// </summary>
    public class ConfigureTOBySMAController : BaseController
    {
        private readonly IConfigureTOBySMAService _configureTOBySMAService;

        public ConfigureTOBySMAController() :
            this(null)
        {
        }

        public ConfigureTOBySMAController(IConfigureTOBySMAService configureTOBySMAService)
        {
            this._configureTOBySMAService = configureTOBySMAService ?? new ConfigureTOBySMAService();
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.ConfigureTOBySMA, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View();
        }
    }
}