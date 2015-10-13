namespace Fanex.Nap.CMS.ColossusBets.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.ColossusBets.Criteria;
    using Fanex.Nap.CMS.ColossusBets.Models;
    using Fanex.Nap.CMS.ColossusBets.Models.API;
    using Fanex.Nap.CMS.ColossusBets.Services;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Infrastructure.Models;
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
            var ticket1 = new Ticket
            {
                Id = 1,
                Member = new IdNameModel { Id = 1176092, Name = "member01@yahoo.com" },
                Currency = new IdNameModel { Id = 1, Name = "RM" },
                Site = new IdNameModel { Id = 1, Name = "Best-Odds" },
                BetId = "1100000002353",
                TotalPayout = 231200,
                Status = new IdNameModel { Id = 1, Name = "Open" },
                SecurityCode = "g5fdsf5178gf3e7"
            };

            var ticket2 = new Ticket
            {
                Id = 2,
                Member = new IdNameModel { Id = 1176092, Name = "member01" },
                Currency = new IdNameModel { Id = 1, Name = "RM" },
                Site = new IdNameModel { Id = 1, Name = "Best-Odds" },
                BetId = "1100000002334",
                TotalPayout = 10000000,
                Status = new IdNameModel { Id = 2, Name = "Info Verified" },
                SecurityCode = "iodm847kfhxlfu5"
            };

            var ticket3 = new Ticket
            {
                Id = 3,
                Member = new IdNameModel { Id = 1176092, Name = "member01" },
                Currency = new IdNameModel { Id = 2, Name = "UUS" },
                Site = new IdNameModel { Id = 1, Name = "Best-Odds" },
                BetId = "1100000002372",
                TotalPayout = 25000000,
                Status = new IdNameModel { Id = 3, Name = "Disbursed" },
                SecurityCode = "958FM7TU83MFLE7"
            };

            var ticket4 = new Ticket
            {
                Id = 4,
                Member = new IdNameModel { Id = 1176092, Name = "member01" },
                Currency = new IdNameModel { Id = 2, Name = "UUS" },
                Site = new IdNameModel { Id = 1, Name = "Best-Odds" },
                BetId = "1100000002372",
                TotalPayout = 25000000,
                Status = new IdNameModel { Id = 4, Name = "Completed" },
                SecurityCode = "958FM7TU83MFLE7"
            };

            var ticket5 = new Ticket
            {
                Id = 5,
                Member = new IdNameModel { Id = 1176092, Name = "member01" },
                Currency = new IdNameModel { Id = 2, Name = "UUS" },
                Site = new IdNameModel { Id = 1, Name = "Best-Odds" },
                BetId = "1100000002372",
                TotalPayout = 25000000,
                Status = new IdNameModel { Id = 5, Name = "Expired" },
                SecurityCode = "958FM7TU83MFLE7"
            };

            var tickets = new List<Ticket> { ticket1, ticket2, ticket3, ticket4, ticket5 };

            //IList<Ticket> tickets = _paymentService.GetTickets(criteria);
            return JsonData(tickets);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ColossusBetsPayoutManagement, Policy = Constant.Policy.Read)]
        public ActionResult GetTicketDetail(GetTicketDetailCriteria criteria)
        {
            //criteria.MemberId = 3529795;
            var task = Task.Factory.StartNew<ColossusBetsAuthorization>(() =>
                ColossusBetsAPIClient.Authorize(CurrentSession.UserId, criteria.MemberId));
            
            var ticketDetailReport = _payoutService.GetTicketDetailReport(criteria);

            ColossusBetsAuthorization authorization = task.Result;
            ticketDetailReport.RefNo = criteria.RefNo;
            ticketDetailReport.CBAuthorization = authorization;

            return PartialView("~/Areas/ColossusBets/Views/Payout/_ViewTicketDetail.cshtml", ticketDetailReport);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ColossusBetsPayoutManagement, Policy = Constant.Policy.Read)]
        public ActionResult GetTicketNotes(GetTicketNotesCriteria criteria)
        {
            var note1 = new TicketNote
            {
                Date = DateTime.Today.AddDays(-5).ToShortFormatedDateWithSlash(),
                Author = "sa",
                Description = "Safe to disbursement"
            };
            var note2 = new TicketNote
            {
                Date = DateTime.Today.AddDays(-2).ToShortFormatedDateWithSlash(),
                Author = "super admin",
                Description = "Transfer done via internet banking"
            };
            var note3 = new TicketNote
            {
                Date = DateTime.Today.ToShortFormatedDateWithSlash(),
                Author = "monitors",
                Description = "Dangeroust!!!"
            };

            var ticketNotes = new List<TicketNote> { note1, note2, note3 };
            // IList<TicketNote> ticketNotes = _payoutService.GetTicketNotes(criteria);
            return PartialView("~/Areas/ColossusBets/Views/Payout/_ViewTicketNotes.cshtml", ticketNotes);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ColossusBetsPayoutManagement, Policy = Constant.Policy.Host)]
        public ActionResult UpdateTicketStatus(UpdateTicketStatusCriteria criteria)
        {
            return Json(new Result(0, "Success."), JsonRequestBehavior.DenyGet);
        }
    }
}
