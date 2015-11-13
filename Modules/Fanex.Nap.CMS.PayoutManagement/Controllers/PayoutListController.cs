namespace Fanex.Nap.CMS.PayoutManagement.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.PayoutManagement.Criteria;
    using Fanex.Nap.CMS.PayoutManagement.Models;
    using Fanex.Nap.CMS.PayoutManagement.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Newtonsoft.Json;

    public class PayoutListController : BaseController
    {
        private readonly PayoutListService _payoutService;

        public PayoutListController()
            : this(null)
        {
        }

        public PayoutListController(PayoutListService payoutService)
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
            ViewBag.Id = id;
            ViewBag.Status = status;
            ViewBag.Statuses = new SelectList(_payoutService.GetStatusStateMachine(status), "Id", "Name");
            ViewBag.PayoutViewSecurityCode = HasRight(CMSFunctionKeys.PayoutViewSecurityCode, Constant.Policy.Read);

            return View("~/Areas/PayoutManagement/Views/PayoutList/PayoutDetail.cshtml");
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.PayoutManagement, Policy = Constant.Policy.Read)]
        public ActionResult GetPayoutDetail(GetTicketNotesCriteria criteria)
        {
            bool viewSecurityCode = HasRight(CMSFunctionKeys.PayoutViewSecurityCode, Constant.Policy.Read);
            var payoutDetail = _payoutService.GetPayoutDetail(criteria, viewSecurityCode);
            return JsonData(payoutDetail);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.PayoutManagement, Policy = Constant.Policy.Host)]
        public ActionResult UpdateTicketStatus(UpdateTicketStatusCriteria criteria)
        {
            var result = _payoutService.UpdateTicketStatus(criteria);
            return JsonData(result, JsonRequestBehavior.DenyGet);
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
                Statuses = _payoutService.GetStatusStateMachine(0),
                UpdateStatus = HasRight(CMSFunctionKeys.PayoutManagement, Constant.Policy.Host)
            };
        }
    }
}
