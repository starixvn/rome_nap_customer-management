// --------------------------------------------------------------------------
// <copyright file="CloseOpenMemberController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.3">Created</change>
// </history>

namespace Fanex.NAP.CMS.Controllers
{
    using System.Web.Mvc;
    using Fanex.NAP.CMS.Services.Implements;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.CMS.ViewModels.OpenCloseMember;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Close open member controller.
    /// </summary>
    public class CloseOpenMemberController : BaseController
    {
        private readonly IOpenCloseMemberService _openCloseMemberService;

        public CloseOpenMemberController() :
            this(null)
        {
        }

        public CloseOpenMemberController(IOpenCloseMemberService openCloseMemberService)
        {
            this._openCloseMemberService = openCloseMemberService ?? new OpenCloseMemberService(this.ServiceContext);
            ViewBag.HasEditAccessRight = this.HasRight(CMSFunctionKeys.Close_OpenMembers, Constant.Policy.Host);
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.Close_OpenMembers, Policy = Constant.Policy.Host)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets member list.
        /// </summary>
        /// <param name="filter">Filter conditions.</param>
        /// <returns>Member list.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.Close_OpenMembers, Policy = Constant.Policy.Host)]
        public JsonResult GetMemberList(GettingMembersStatusFilter filter)
        {
            GettingMembersStatusResult result = this._openCloseMemberService.GetMembersStatus(filter);

            return Json(result);
        }

        /// <summary>
        /// Updates member status.
        /// </summary>
        /// <param name="filter">Filter conditions.</param>
        /// <returns>JSON object indicates the result of updating process.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.Close_OpenMembers, Policy = Constant.Policy.Host)]
        public JsonResult UpdateMembers(UpdatedMembersStatusFilter filter)
        {
            UpdatedMembersStatusResult result = null;

            filter.AdminId = this.CurrentSession.UserId;
            filter.AdminUsername = this.CurrentSession.Username;
            filter.IP = this.CurrentContext.IP;
            result = this._openCloseMemberService.UpdateMembersStatus(filter);

            return Json(result);
        }
    }
}