namespace Fanex.Nap.CMS.ColossusBets.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.ColossusBets.Criteria;
    using Fanex.Nap.CMS.ColossusBets.Enums;
    using Fanex.Nap.CMS.ColossusBets.Models;
    using Fanex.Nap.CMS.ColossusBets.Resources;
    using Fanex.Nap.CMS.ColossusBets.Services;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.NAP.Core.Business;

    public class PaymentController : BaseController
    {
        private readonly PaymentService _paymentService;

        public PaymentController()
            : this(null)
        {
        }

        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService ?? new PaymentService();
        }

        public ActionResult Index()
        {
            PopulateStatusAndPermissions();
            return View("~/Areas/ColossusBets/Views/Payment/Index.cshtml");
        }

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
                Status = "Pending",
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
                Status = "Disbursement",
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
                Status = "Completed",
                SecurityCode = "958fm7tu83mfle7"
            };

            var tickets = new List<Ticket> { ticket1, ticket2, ticket3 };

            //criteria.ShowSecurityCode = HasRight(CMSFunctionKeys.ColossusBetsPayoutShowSecurityCode, Constant.Policy.Read);
            //IList<Ticket> tickets = _paymentService.GetTickets(criteria);
            return JsonData(tickets);
        }

        public ActionResult GetTicketDetail(GetTicketDetailCriteria criteria)
        {
            var ticketDetailReport = _paymentService.GetTicketDetailReport(criteria);
            return JsonData(ticketDetailReport);
        }

        public ActionResult GetTicketNotes(GetTicketNotesCriteria criteria)
        {
            /*var note1 = new TicketNote
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

            var ticketNotes = new List<TicketNote> { note1, note2, note3 };*/
            IList<TicketNote> ticketNotes = _paymentService.GetTicketNotes(criteria);
            return JsonData(ticketNotes);
        }

        [HttpPost]
        public ActionResult UpdateTicketStatus(UpdateTicketStatusCriteria criteria)
        {
            return Json(new Result(0, "Success."), JsonRequestBehavior.DenyGet);
        }

        private void PopulateStatusAndPermissions()
        {
            IList<IdNameModel> statuses = new List<IdNameModel> 
            {
                new IdNameModel { Id = (int)PaymentStatus.Pending, Name = PaymentStatus.Pending.ToString() },
                new IdNameModel { Id = (int)PaymentStatus.Disbursement, Name = PaymentStatus.Disbursement.ToString() },
                new IdNameModel { Id = (int)PaymentStatus.Completed, Name = PaymentStatus.Completed.ToString() }
            };

            IList<IdNameModel> statusesSearch = statuses.Select(s => new IdNameModel { Id = s.Id, Name = s.Name }).ToList();
            statusesSearch.Insert(0, new IdNameModel { Id = 0, Name = ColossusBetsLabels.All });

            ViewBag.StatusListForFilter = new SelectList(statusesSearch, "Id", "Name");
            ViewBag.StatusList = new SelectList(statuses, "Name", "Name");

            ViewBag.UpdateStatus = true; // HasRight(CMSFunctionKeys.ColossusBetsPayoutUpdateTicketStatus, Constant.Policy.Host);
            ViewBag.ShowSecurityCode = true; // HasRight(CMSFunctionKeys.ColossusBetsPayoutShowSecurityCode, Constant.Policy.Read);
        }
    }
}
