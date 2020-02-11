namespace Fanex.Nap.CMS.Customers.Controllers
{
    using System.Web.Mvc;
    using Constants;
    using Fanex.NAP.Core.Configuration;
    using Infrastructure;
    using Infrastructure.Controllers;
    using Infrastructure.Extensions;
    using NAP.Core.Business;
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

        public ActionResult AbnormalBetTicket(int custId, string custName, SearchViewModel searchViewModel, byte siteType = 0)
        {
            if (HasReadManageProblem(siteType))
            {
                var isCN88 = SiteTypeConst.IsCN88(siteType);
                TempData[SearchFilterSessionKeyConst.GetSessionKey(isCN88)] = searchViewModel;

                const int pageIndex = 1;
                var fromDate = searchViewModel.From;
                var toDate = searchViewModel.To.GetEndOfDate();
                var abnormalBetTicketData = service.GetAbnormalBetTickets(custId, fromDate, toDate, pageIndex, searchViewModel.PageSize, isCN88);

                var vm = new AbnormalBetTicketViewModel
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    CustId = custId,
                    CustName = custName,
                    PageIndex = pageIndex,
                    PageSize = searchViewModel.PageSize,
                    SiteType = siteType,
                    TotalAbnormalBetTickets = abnormalBetTicketData.Item1,
                    AbnormalBetTickets = abnormalBetTicketData.Item2,
                    TotalSportsbookBetTickets = service.GetTotalSporksbookBetTickets(custId, fromDate, toDate, isCN88)
                };

                return View("~/Areas/Customers/Views/BetList/AbnormalBetTicket.cshtml", vm);
            }

            return Redirect(NapSettings.Settings.UnauthorizeUrl);
        }

        public ActionResult GetAbnormalBetTicketPaging(AbnormalBetTicketViewModel viewModel)
        {
            if (HasReadManageProblem(viewModel.SiteType))
            {
                return JsonData(service.GetAbnormalBetTickets(
                    viewModel.CustId,
                    viewModel.FromDate,
                    viewModel.ToDate.GetEndOfDate(),
                    viewModel.PageIndex,
                    viewModel.PageSize,
                    SiteTypeConst.IsCN88(viewModel.SiteType)));
            }

            return Redirect(NapSettings.Settings.UnauthorizeUrl);
        }

        private bool HasReadManageProblem(byte siteType)
            => HasRight(GetFunctionKey(siteType), Constant.Policy.Read);

        private static string GetFunctionKey(byte siteType)
        {
            if (SiteTypeConst.IsCN88(siteType))
            {
                return CMSFunctionKeys.ManageProblemAccountCN88;
            }

            return SiteTypeConst.IsCredit(siteType)
                ? CMSFunctionKeys.ManageProblemAccountCreditSites
                : CMSFunctionKeys.ManageProblemAccountDepositSites;
        }
    }
}
