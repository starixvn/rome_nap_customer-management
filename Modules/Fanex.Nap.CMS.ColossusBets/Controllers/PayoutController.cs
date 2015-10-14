namespace Fanex.Nap.CMS.ColossusBets.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.ColossusBets.Criteria;
    using Fanex.Nap.CMS.ColossusBets.Models.API;
    using Fanex.Nap.CMS.ColossusBets.Services;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Newtonsoft.Json;

    public class PayoutController : BaseController
    {
        private readonly PayoutService _payoutService;

        public PayoutController()
            : this(null)
        {
        }

        public PayoutController(PayoutService payoutService)
        {
            _payoutService = payoutService ?? new PayoutService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ColossusBetsPayoutManagement, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var statusStateMachine = _payoutService.GetStatusStateMachine();
            ViewBag.StatusListForFilter = new SelectList(statusStateMachine[0], "Id", "Name");
            
            string jsonString = JsonConvert.SerializeObject(
                statusStateMachine,
                Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            return View("~/Areas/ColossusBets/Views/Payout/Index.cshtml", null, jsonString);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ColossusBetsPayoutManagement, Policy = Constant.Policy.Read)]
        public ActionResult GetTickets(GetTicketsCriteria criteria)
        {
            var tickets = _payoutService.GetTickets(criteria);
            return JsonData(tickets);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ColossusBetsPayoutManagement, Policy = Constant.Policy.Read)]
        public ActionResult GetTicketDetail(GetTicketDetailCriteria criteria)
        {
            int userId = CurrentSession.UserId;
            var task = Task.Factory.StartNew<ColossusBetsAuthorization>(() =>
                ColossusBetsAPIClient.Authorize(userId, criteria.MemberId));
            
            var ticketDetailReport = _payoutService.GetTicketDetailReport(criteria);

            ColossusBetsAuthorization authorization = task.Result;
            ticketDetailReport.BetId = criteria.BetId;
            ticketDetailReport.CBAuthorization = authorization;

            return PartialView("~/Areas/ColossusBets/Views/Payout/_ViewTicketDetail.cshtml", ticketDetailReport);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ColossusBetsPayoutManagement, Policy = Constant.Policy.Read)]
        public ActionResult GetTicketNotes(GetTicketNotesCriteria criteria)
        {
            var ticketNotes = _payoutService.GetTicketNotes(criteria);
            return PartialView("~/Areas/ColossusBets/Views/Payout/_ViewTicketNotes.cshtml", ticketNotes);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ColossusBetsPayoutManagement, Policy = Constant.Policy.Host)]
        public ActionResult UpdateTicketStatus(UpdateTicketStatusCriteria criteria)
        {
            var result = _payoutService.UpdateTicketStatus(criteria);
            return JsonData(result, JsonRequestBehavior.DenyGet);
        }
    }
}
