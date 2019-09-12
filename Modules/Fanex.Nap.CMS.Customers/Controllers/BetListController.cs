namespace Fanex.Nap.CMS.Customers.Controllers
{
    using System;
    using System.Web.Mvc;
    using Infrastructure;
    using Infrastructure.Controllers;
    using Infrastructure.Extensions;
    using Infrastructure.Web;
    using NAP.Core.Business;
    using NAP.Core.Web.Attribute;
    using Services;
    using ViewModels.BetList;
    using ViewModels.ProblemAccount;

    public class BetListController : BaseController
    {
        private readonly IBetListService service;

        public BetListController()
            : this(null)
        {
        }

        public BetListController(IBetListService service)
        {
            this.service = service ?? new BetListService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCN88, Policy = Constant.Policy.Read)]
        public ActionResult AbnormalBetTicket(int custId, string custName, SearchViewModel searchViewModel)
        {
            TempData[ProblemAccountController.SearchFilterSessionKey] = searchViewModel;

            var fromDate = searchViewModel.From;
            var toDate = searchViewModel.To.GetEndOfDate();
            var abnormalBetTicketData = service.GetAbnormalBetTickets(custId, fromDate, toDate, 1, searchViewModel.PageSize);
            var totalSporksbookBetTickets = service.GetTotalSporksbookBetTickets(custId, fromDate, toDate, AppSession.BalanceUpDate.MoveBetsDay);

            var vm = new AbnormalBetTicketViewModel(
                fromDate,
                toDate,
                custId,
                custName,
                abnormalBetTicketData.Item1,
                abnormalBetTicketData.Item2,
                totalSporksbookBetTickets,
                searchViewModel.PageSize);

            return View("~/Areas/Customers/Views/BetList/AbnormalBetTicket.cshtml", vm);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCN88, Policy = Constant.Policy.Read)]
        public ActionResult GetAbnormalBetTicketPaging(int custId, DateTime fromDate, DateTime toDate, int pageIndex, int pageSize)
            => JsonData(service.GetAbnormalBetTickets(custId, fromDate, toDate.GetEndOfDate(), pageIndex, pageSize));
    }
}
