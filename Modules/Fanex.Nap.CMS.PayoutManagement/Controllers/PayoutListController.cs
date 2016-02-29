namespace Fanex.Nap.CMS.PayoutManagement.Controllers
{
    using Constants;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.PayoutManagement.Criteria;
    using Fanex.Nap.CMS.PayoutManagement.Enums;
    using Fanex.Nap.CMS.PayoutManagement.Models;
    using Fanex.Nap.CMS.PayoutManagement.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Infrastructure.Web;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class PayoutListController : BaseController
    {
        private readonly IPayoutListService _payoutService;

        public PayoutListController()
            : this(null)
        {
        }

        public PayoutListController(IPayoutListService payoutService)
        {
            _payoutService = payoutService ?? new PayoutListService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.PayoutManagement, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            string jsonString = JsonConvert.SerializeObject(
                GetPayoutListFilter(),
                Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return View("~/Areas/PayoutManagement/Views/PayoutList/Index.cshtml", null, jsonString);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.PayoutManagement, Policy = Constant.Policy.Read)]
        public ActionResult GetPayoutList(GetPayoutListCriteria criteria)
        {
            var tickets = _payoutService.GetPayoutList(criteria);
            return JsonData(tickets);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.PayoutManagement, Policy = Constant.Policy.Read)]
        public ActionResult PayoutDetail(int id, int status)
        {
            bool updateTicketStatus = false;
            bool viewSecurityCode = HasRight(CMSFunctionKeys.PayoutViewWinningCode, Constant.Policy.Read);
            
            var criteria = new GetPayoutDetailCriteria { Id = id };
            var payoutDetail = _payoutService.GetPayoutDetail(criteria, viewSecurityCode);

            string groupSitenamePermissionKey = _payoutService.GetGroupSitenamePermissionKey(payoutDetail.Site);
            if (!string.IsNullOrEmpty(groupSitenamePermissionKey))
            {
                updateTicketStatus = HasRight(groupSitenamePermissionKey, Constant.Policy.Host);
            }

            payoutDetail.Id = id;
            payoutDetail.PayoutNotes = null;
            ViewBag.ViewSecuriyCodePermission = viewSecurityCode;
            ViewBag.UpdateTicketStatusPermission = updateTicketStatus;
            
            ViewBag.Status = status;
            ViewBag.Statuses = new SelectList(GetStatuses(), "Id", "Name");

            string jsonString = JsonConvert.SerializeObject(
                payoutDetail,
                Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return View("~/Areas/PayoutManagement/Views/PayoutList/PayoutDetail.cshtml", null, jsonString);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.PayoutManagement, Policy = Constant.Policy.Read)]
        public ActionResult GetPayoutDetail(GetPayoutDetailCriteria criteria)
        {
            bool viewSecurityCode = HasRight(CMSFunctionKeys.PayoutViewWinningCode, Constant.Policy.Read);
            var payoutDetail = _payoutService.GetPayoutDetail(criteria, viewSecurityCode);
            SessionManager.Set(PayoutManagementConst.WinningCode, payoutDetail.SecurityCode);
            return JsonData(payoutDetail.PayoutNotes);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.PayoutManagement, Policy = Constant.Policy.Host)]
        public ActionResult UpdateTicketStatus(UpdateTicketStatusCriteria criteria)
        {
            var result = _payoutService.UpdateTicketStatus(criteria);
            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.PayoutManagement, Policy = Constant.Policy.Read)]
        public ActionResult CheckWinningCode(string winningCode)
        {
            return JsonData(winningCode.Equals(SessionManager.Get<string>(PayoutManagementConst.WinningCode)));
        }

        [NonAction]
        private dynamic GetPayoutListFilter()
        {
            IList<GroupIdNameModel> sitesLicensees;
            IList<IdNameModel> products;
            _payoutService.GetSitesLicenseesProducts(out sitesLicensees, out products);

            return new 
            {
                SitesLicensees = sitesLicensees,
                Products = products,
                Statuses = GetStatuses()
            };
        }

        [NonAction]
        private IList<IdNameModel> GetStatuses()
        {
            return new List<IdNameModel>(5) 
            {
                new IdNameModel { Id = (int)PayoutStatus.Open, Name = PayoutStatus.Open.ToString() },
                new IdNameModel { Id = (int)PayoutStatus.Verified, Name = PayoutStatus.Verified.ToString() },
                new IdNameModel { Id = (int)PayoutStatus.Disbursed, Name = PayoutStatus.Disbursed.ToString() },
                new IdNameModel { Id = (int)PayoutStatus.Completed, Name = PayoutStatus.Completed.ToString() },
                new IdNameModel { Id = (int)PayoutStatus.Expired, Name = PayoutStatus.Expired.ToString() }
            };
        }
    }
}
