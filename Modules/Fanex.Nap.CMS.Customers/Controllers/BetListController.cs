namespace Fanex.Nap.CMS.Customers.Controllers
{
    using System.Web.Mvc;
    using Constants.ProblemAccount;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Web.Attribute;
    using Infrastructure;
    using Infrastructure.Controllers;
    using Infrastructure.Extensions;
    using NAP.Core.Business;
    using Services.ProblemAccount;
    using ViewModels.BetList;
    using ViewModels.ProblemAccount;
    using ViewModels.ProblemAccountCreditSite;
    using ViewModels.ProblemAccountLicenseeSite;

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
            TempData[SearchFilterSessionKeyConst.CN88] = searchViewModel;

            return GetAbnormalBetTicket(custId, custName, searchViewModel, SiteTypeConst.CN88);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCreditSites, Policy = Constant.Policy.Read)]
        public ActionResult AbnormalBetTicketCreditSite(int custId, string custName, SearchCreditSiteViewModel searchViewModel)
        {
            TempData[SearchFilterSessionKeyConst.Credit] = searchViewModel;

            return GetAbnormalBetTicket(custId, custName, searchViewModel, SiteTypeConst.Credit);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountLicenseeSites, Policy = Constant.Policy.Read)]
        public ActionResult AbnormalBetTicketLicenseeSite(int custId, string custName, SearchLicenseeSiteViewModel searchViewModel)
        {
            TempData[SearchFilterSessionKeyConst.Licensee] = searchViewModel;

            return GetAbnormalBetTicket(custId, custName, searchViewModel, SiteTypeConst.Licensee);
        }

        [NonAction]
        private ActionResult GetAbnormalBetTicket(int custId, string custName, SearchBaseViewModel searchViewModel, byte siteType)
        {
            const int pageIndex = 1;
            var fromDate = searchViewModel.From;
            var toDate = searchViewModel.To.GetEndOfDate();
            var isCN88 = SiteTypeConst.IsCN88(siteType);
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
                : CMSFunctionKeys.ManageProblemAccountLicenseeSites;
        }
    }
}
