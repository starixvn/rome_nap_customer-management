// --------------------------------------------------------------------------
// <copyright file="CloseOpenMemberController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.3">Created</change>
// </history>

namespace Fanex.NAP.Miscellany.Controllers
{
    using System.Web.Mvc;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Close open member controller.
    /// </summary>
    public class CloseOpenMemberController : BaseController
    {
        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = MiscellanyFunctionKeys.Close_OpenMembers, Policy = Constant.Policy.Host)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get member list.
        /// </summary>
        /// <returns>Member list.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = MiscellanyFunctionKeys.Close_OpenMembers, Policy = Constant.Policy.Host)]
        public JsonResult GetMemberList()
        {
            return Json(null);
        }

        /// <summary>
        /// Update member status.
        /// </summary>
        /// <returns>JSON object indicates the result of updating process.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = MiscellanyFunctionKeys.Close_OpenMembers, Policy = Constant.Policy.Host)]
        public JsonResult UpdateMembers()
        {
            return Json(null);
        }
    }
}