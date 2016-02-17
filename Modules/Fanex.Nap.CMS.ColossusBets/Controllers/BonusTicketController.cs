namespace Fanex.Nap.CMS.ColossusBets.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Criteria;
    using Data.Repository;
    using Enums;
    using Infrastructure;
    using Infrastructure.Controllers;
    using Infrastructure.Models;
    using Models;
    using NAP.Core.Business;
    using NAP.Core.Web.Attribute;
    using Newtonsoft.Json;
    using Services;

    public class BonusTicketController : BaseController
    {
        private readonly IBonusTicketService _bonusTicketService;

        public BonusTicketController()
        {
            _bonusTicketService = new BonusTicketService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ColossusBetsInsertBonusTicket, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            ViewBag.HostPermission = HasRight(CMSFunctionKeys.ColossusBetsInsertBonusTicket, Constant.Policy.Host);

            ICriteria criteria = new GetBonusTicketCriteria();
            var bonusTickets = _bonusTicketService.GetBonusTickets(criteria).ToList();

            return View("~/Areas/ColossusBets/Views/BonusTicket/Index.cshtml", null, SerializeData(bonusTickets));
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ColossusBetsInsertBonusTicket, Policy = Constant.Policy.Read)]
        public ActionResult BonusTicketDetail(long? refno)
        {
            PopulateStatusSelect();
            ViewBag.HostPermission = HasRight(CMSFunctionKeys.ColossusBetsInsertBonusTicket, Constant.Policy.Host);

            BonusTicketDetail model =
                (!refno.HasValue || refno.Value <= 0)
                ? new BonusTicketDetail { Choice = "1-1" }
                : _bonusTicketService.GetBonusTicketDetail(refno.Value);

            return View("~/Areas/ColossusBets/Views/BonusTicket/BonusTicketDetail.cshtml", model);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ColossusBetsInsertBonusTicket, Policy = Constant.Policy.Read)]
        public ActionResult BonusTicketDetailByRefNo(long refno)
        {
            var model = _bonusTicketService.GetBonusTicketDetailByRefNo(refno);

            return JsonData(model);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ColossusBetsInsertBonusTicket, Policy = Constant.Policy.Host)]
        public ActionResult SaveBonusTicket(SaveBonusTicketCriteria criteria)
        {
            var bonusTicketStatus = (BonusTicketStatus)criteria.StatusId;

            criteria.Status = bonusTicketStatus.ToString();
            criteria.UserId = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;

            var result = _bonusTicketService.SaveBonusTicket(criteria);

            if (bonusTicketStatus != BonusTicketStatus.Running && result.Code == 0)
            {
                var bonusTicketDetail = _bonusTicketService.GetBonusTicketDetailByRefNo(criteria.RefNo);

                decimal amount = bonusTicketStatus == BonusTicketStatus.WON ? criteria.WinAmount : 0;
                var resultAPI = _bonusTicketService.DoSettlementAPI(bonusTicketDetail.EventId, bonusTicketDetail.TranId3rd, criteria.CustId, amount);

                if (resultAPI.Code != 0)
                {
                    result = resultAPI;
                }
            }

            return JsonData(result);
        }

        [NonAction]
        private string SerializeData(object data)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore
            };

            return JsonConvert.SerializeObject(data, settings);
        }

        [NonAction]
        private void PopulateStatusSelect()
        {
            IList<IdNameModel> statuses = new List<IdNameModel>
            {
                new IdNameModel { Id = (int)BonusTicketStatus.Running, Name = BonusTicketStatus.Running.ToString() },
                new IdNameModel { Id = (int)BonusTicketStatus.WON, Name = BonusTicketStatus.WON.ToString() },
                new IdNameModel { Id = (int)BonusTicketStatus.LOSE, Name = BonusTicketStatus.LOSE.ToString() }
            };

            ViewBag.Status = new SelectList(statuses, "Id", "Name"); ;
        }
    }
}
