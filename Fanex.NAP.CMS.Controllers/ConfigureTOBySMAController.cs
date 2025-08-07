// --------------------------------------------------------------------------
// <copyright file="ConfigureTOBySMAController.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.16">Created</change>
// </history>

namespace Fanex.NAP.CMS.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.NAP.CMS.Core;
    using Fanex.NAP.CMS.Criteria.ConfigureTOBySMA;
    using Fanex.NAP.CMS.Services.Implements;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.CMS.ViewModels.ConfigureTOBySMA;
    using Fanex.NAP.Client.Helpers.Business;
    using Fanex.NAP.Client.Attributes;
    using Fanex.NAP.CMS.Models.Common;

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
        [NapAuth(FunctionKey = CMSFunctionKeys.ControlTOBySMA, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            ViewBag.HasEditAccessRight = this.HasRight(CMSFunctionKeys.ControlTOBySMA, Constant.Policy.Host);
            
            return View();
        }

        /// <summary>
        /// Gets SMA offset.
        /// </summary>
        /// <param name="criteria">The criteria that are used to get SMA offset.</param>
        /// <returns>List of JSON objects represents the SMA offset.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ControlTOBySMA, Policy = Constant.Policy.Read)]
        public JsonResult GetSMAOffset(GetSMAOffsetCriteria criteria)
        {
            IEnumerable<SMAOffset> result = null;

            criteria.SMAName = criteria.SMAName ?? string.Empty;
            result = this._configureTOBySMAService.GetSMAOffset(criteria);

            return Json(result);
        }

        /// <summary>
        /// Inserts the SMA offset.
        /// </summary>
        /// <param name="filter">The filter that is used to insert SMA offset.</param>
        /// <returns>List of JSON objects represents the updated SMA offset.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ControlTOBySMA, Policy = Constant.Policy.Host)]
        public JsonResult InsertSMAOffset(InsertSMAOffsetFilter filter)
        {
            var insertCriteria = new InsertSMAOffsetCriteria
            {
                SMAName = filter.SMAName,
                Offset = filter.Value,
                UserID = this.CurrentSession.UserId,
                IP = this.CurrentContext.IP
            };
            ICommandResult insertedResult = this._configureTOBySMAService.InsertSMAOffset(insertCriteria);
            IEnumerable<SMAOffset> updatedOffset = null;

            if (insertedResult.IsSuccessful)
            {
                filter.SearchedName = filter.SearchedName ?? string.Empty;
                updatedOffset = this._configureTOBySMAService.GetSMAOffset(new GetSMAOffsetCriteria
                {
                    SMAName = filter.SearchedName
                });
            }

            return Json(new UpdatedSMAOffsetResult
            {
                ErrorCode = insertedResult.Code,
                ErrorMessage = insertedResult.Message,
                OffsetItems = updatedOffset
            });
        }

        /// <summary>
        /// Updates the SMA offset.
        /// </summary>
        /// <param name="filter">The filter that is used to update SMA offset.</param>
        /// <returns>List of JSON objects represents the updated SMA offset.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ControlTOBySMA, Policy = Constant.Policy.Host)]
        public JsonResult UpdateSMAOffset(UpdateSMAOffsetFilter filter)
        {
            var updateCriteria = new UpdateSMAOffsetCriteria
            {
                SMAID = filter.CustID,
                Offset = filter.Value,
                UserID = this.CurrentSession.UserId,
                IP = this.CurrentContext.IP
            };
            ICommandResult updatedResult = this._configureTOBySMAService.UpdateSMAOffset(updateCriteria);
            IEnumerable<SMAOffset> updatedOffset = null;

            if (updatedResult.IsSuccessful)
            {
                filter.SearchedName = filter.SearchedName ?? string.Empty;
                updatedOffset = this._configureTOBySMAService.GetSMAOffset(new GetSMAOffsetCriteria
                {
                    SMAName = filter.SearchedName
                });
            }

            return Json(new UpdatedSMAOffsetResult
            {
                ErrorCode = updatedResult.Code,
                ErrorMessage = updatedResult.Message,
                OffsetItems = updatedOffset
            });
        }

        /// <summary>
        /// Deletes the SMA offset.
        /// </summary>
        /// <param name="filter">The filter that is used to delete SMA offset.</param>
        /// <returns>List of JSON objects represents the updated SMA offset.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ControlTOBySMA, Policy = Constant.Policy.Host)]
        public JsonResult DeleteSMAOffset(DeleteSMAOffsetFilter filter)
        {
            var deleteCriteria = new DeleteSMAOffsetCriteria
            {
                SMAID = filter.CustID,
                UserID = this.CurrentSession.UserId,
                IP = this.CurrentContext.IP
            };
            ICommandResult deletedResult = this._configureTOBySMAService.DeleteSMAOffset(deleteCriteria);
            IEnumerable<SMAOffset> updatedOffset = null;

            if (deletedResult.IsSuccessful)
            {
                filter.SearchedName = filter.SearchedName ?? string.Empty;
                updatedOffset = this._configureTOBySMAService.GetSMAOffset(new GetSMAOffsetCriteria
                {
                    SMAName = filter.SearchedName
                });
            }

            return Json(new UpdatedSMAOffsetResult
            {
                ErrorCode = deletedResult.Code,
                ErrorMessage = deletedResult.Message,
                OffsetItems = updatedOffset
            });
        }
    }
}