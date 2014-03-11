// --------------------------------------------------------------------------
// <copyright file="ConfigureCustomerAccessController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.6">Created</change>
// </history>

namespace Fanex.NAP.Miscellany.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Framework.Command;
    using Fanex.NAP.Miscellany.Criteria.ConfigureCustomerAccess;
    using Fanex.NAP.Miscellany.Services.Implements;
    using Fanex.NAP.Miscellany.Services.Interfaces;
    using Fanex.NAP.Miscellany.ViewModels.ConfigureCustomerAccess;

    /// <summary>
    /// Allow Access Management controller.
    /// </summary>
    public class ConfigureCustomerAccessController : BaseController
    {
        private readonly IConfigureCustomerAccessService _accessManagementService;

        public ConfigureCustomerAccessController() :
            this(null)
        {
        }

        public ConfigureCustomerAccessController(IConfigureCustomerAccessService accessManagementService)
        {
            this._accessManagementService = accessManagementService ?? new ConfigureCustomerAccessService();
            ViewBag.HasEditAccessRight = this.HasRight(MiscellanyFunctionKeys.ConfigureCustomerAccess, Constant.Policy.Host);
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = MiscellanyFunctionKeys.ConfigureCustomerAccess, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets request form for inserting and updating request purpose.
        /// </summary>
        /// <returns>Partial view represent request form.</returns>
        [ChildActionOnly]
        public PartialViewResult RequestForm()
        {
            Dictionary<string, string> countryList = (this._accessManagementService as ConfigureCustomerAccessService).CountryList;
            IEnumerable<SelectListItem> countrySelectList = countryList.Select(obj => new SelectListItem
            {
                Text = obj.Value,
                Value = obj.Key
            });

            ViewBag.CountryList = countrySelectList;

            return PartialView();
        }

        /// <summary>
        /// Gets request list.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of JSON objects.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = MiscellanyFunctionKeys.ConfigureCustomerAccess, Policy = Constant.Policy.Read)]
        public JsonResult GetRequestList(GetRequestInfoListCriteria criteria)
        {
            IEnumerable<RequestItem> requestList = this._accessManagementService.GetRequestList(criteria);

            return Json(new
            {
                RequestList = requestList,
                TotalItems = criteria.TotalItems
            });
        }

        /// <summary>
        /// Gets an existing request.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>JSON object indicates the data of queried request.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = MiscellanyFunctionKeys.ConfigureCustomerAccess, Policy = Constant.Policy.Host)]
        public JsonResult GetRequest(GetRequestInfoCriteria criteria)
        {
            return Json(this._accessManagementService.GetRequest(criteria));
        }

        /// <summary>
        /// Inserts new request.
        /// </summary>
        /// <param name="criteria">The criteria contain data of new request.</param>
        /// <returns>JSON object indicates the new request is inserted successfully or not.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = MiscellanyFunctionKeys.ConfigureCustomerAccess, Policy = Constant.Policy.Host)]
        public JsonResult InsertRequest(InsertRequestCriteria criteria)
        {
            ICommandResult result = null;

            criteria.CountryCode = criteria.CountryCode ?? string.Empty;
            criteria.IPRequest = criteria.IPRequest ?? string.Empty;
            criteria.Creator = this.CurrentSession.UserId;
            criteria.IPAction = this.CurrentContext.IP;
            result = this._accessManagementService.InsertRequest(criteria);

            return Json(result);
        }

        /// <summary>
        /// Updates an existing request.
        /// </summary>
        /// <param name="criteria">The criteria contain updated data the request.</param>
        /// <returns>JSON object indicates the new request is updated successfully or not.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = MiscellanyFunctionKeys.ConfigureCustomerAccess, Policy = Constant.Policy.Host)]
        public JsonResult UpdateRequest(UpdateRequestCriteria criteria)
        {
            ICommandResult result = null;

            criteria.CountryCode = criteria.CountryCode ?? string.Empty;
            criteria.IPRequest = criteria.IPRequest ?? string.Empty;
            criteria.UpdatedBy = this.CurrentSession.UserId;
            criteria.IPAction = this.CurrentContext.IP;
            result = this._accessManagementService.UpdateRequest(criteria);

            return Json(result);
        }

        /// <summary>
        /// Deletes an existing request.
        /// </summary>
        /// <param name="criteria">The criteria contain updated data the request.</param>
        /// <returns>JSON object indicates the new request is updated successfully or not.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = MiscellanyFunctionKeys.ConfigureCustomerAccess, Policy = Constant.Policy.Host)]
        public JsonResult DeleteRequest(DeleteRequestCriteria criteria)
        {
            ICommandResult result = this._accessManagementService.DeleteRequest(criteria);
            return Json(result);
        }
    }
}