namespace Fanex.Nap.CMS.PayoutManagement.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Constants;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.PayoutManagement.Criteria;
    using Fanex.Nap.CMS.PayoutManagement.Models.API;
    using Fanex.Nap.CMS.PayoutManagement.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Infrastructure.Web;

    public class BetTicketController : BaseController
    {
        private readonly IBetTicketService _betTicketService;

        public BetTicketController()
            : this(null)
        {
        }

        public BetTicketController(IBetTicketService betTicketService)
        {
            _betTicketService = betTicketService ?? new BetTicketService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.PayoutManagement, Policy = Constant.Policy.Read)]
        public ActionResult Microgaming(GetBetTicketCriteria criteria)
        {
            var microgaming = _betTicketService.GetMicrogaming(criteria);
            var microgamingResult = PayoutManagementAPIClient.MicrogamingResult(
                microgaming.Sitename,
                microgaming.CustomerId,
                microgaming.TransId3rd);

            return PartialView("~/Areas/PayoutManagement/Views/BetTicket/Microgaming.cshtml", microgamingResult);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.PayoutManagement, Policy = Constant.Policy.Read)]
        public ActionResult ColossusBets(GetBetTicketCriteria criteria)
        {
            int userId = CurrentSession.UserId;
            var task = Task.Factory.StartNew<ColossusBetsAuthorization>(
                () => PayoutManagementAPIClient.ColossusBetsAuthorize(userId, criteria.Member));

            var colossusbets = _betTicketService.GetColossusBets(criteria);

            string siteIdByCustomer = _betTicketService.GetSiteIdByCustomerId(new GetSiteIdByCustomerIdCriteria(criteria.Member));
            ViewBag.CBDomain = PayoutManagementConst.GetColossusBetsDomainBySiteId(siteIdByCustomer);
            ViewBag.CBAuthorization = task.Result;

            return PartialView("~/Areas/PayoutManagement/Views/BetTicket/ColossusBets.cshtml", colossusbets);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.PayoutManagement, Policy = Constant.Policy.Read)]
        public ActionResult RNGCasinoJackpot(GetJackpotBetTicketCriteria criteria)
        {
            var balanceUpdate = AppSession.BalanceUpDate;
            criteria.MoveBetsDay = balanceUpdate.MoveBetsDay;
            criteria.BalanceUpDay = balanceUpdate.BalanceUpDay;

            var refno2 = _betTicketService.GetRNGCasinoJackpotRefNo2(criteria);

            return PartialView("~/Areas/PayoutManagement/Views/BetTicket/RNGCasinoJackpot.cshtml", refno2);
        }
    }
}
