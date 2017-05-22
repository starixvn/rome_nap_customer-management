// --------------------------------------------------------------------------
// <copyright file="GlobalSettingController.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.Nap.CMS.TurnoverControl.Controllers
{
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting;
    using Fanex.Nap.CMS.TurnoverControl.Services.GlobalSetting;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Global Setting Controller.
    /// </summary>
    public class GlobalSettingController : BaseController
    {
        private readonly IGlobalSettingService _globalSettingService;

        public GlobalSettingController()
            : this(null)
        {
        }

        public GlobalSettingController(IGlobalSettingService globalSettingService)
        {
            _globalSettingService = globalSettingService ?? new GlobalSettingService();
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.GlobalSetting, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View("~/Areas/TurnoverControl/Views/GlobalSetting/Index.cshtml");
        }

        /// <summary>
        /// Redirects to Open Add New Setting view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.GlobalSetting, Policy = Constant.Policy.Host)]
        public ActionResult OpenAddGlobalSettingView()
        {
            return View("~/Areas/TurnoverControl/Views/GlobalSetting/AddSetting/Index.cshtml");
        }

        /// <summary>
        /// Gets global setting list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// Global setting list.
        /// </returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.GlobalSetting, Policy = Constant.Policy.Read)]
        public JsonResult GlobalSettingList(GlobalSettingListCriteria criteria)
        {
            var result = _globalSettingService.GlobalSettingList(criteria);
            return Json(result);
        }

        /// <summary>
        /// Sort global setting list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// Global setting list.
        /// </returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.GlobalSetting, Policy = Constant.Policy.Read)]
        public JsonResult SortGlobalSettingList(GlobalSettingListCriteria criteria)
        {
            var result = _globalSettingService.SortGlobalSettingList(criteria);
            return Json(result);
        }

        /// <summary>
        /// Export global setting list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        [NapAuth(FunctionKey = CMSFunctionKeys.GlobalSetting, Policy = Constant.Policy.Read)]
        public void ExportGlobalSettingList(GlobalSettingListCriteria criteria)
        {
            _globalSettingService.ExportGlobalSettingList(criteria);
        }

        /// <summary>
        /// Edit global setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// Edit result.
        /// </returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.GlobalSetting, Policy = Constant.Policy.Host)]
        public JsonResult EditGlobalSetting(EditGlobalSettingCriteria criteria)
        {
            var result = _globalSettingService.EditGlobalSetting(criteria);
            return Json(result);
        }

        /// <summary>
        /// Delete global setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// Delete result.
        /// </returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.GlobalSetting, Policy = Constant.Policy.Host)]
        public JsonResult DeleteGlobalSetting(DeleteGlobalSettingCriteria criteria)
        {
            var result = _globalSettingService.DeleteGlobalSetting(criteria);
            return Json(result);
        }

        /// <summary>
        /// Add global setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// Add result.
        /// </returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.GlobalSetting, Policy = Constant.Policy.Host)]
        public JsonResult AddGlobalSetting(AddGlobalSettingCriteria criteria)
        {
            var result = _globalSettingService.AddGlobalSetting(criteria);
            return Json(result);
        }
    }
}
