namespace Fanex.Nap.CMS.Customers.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Constants.ProblemAccount;
    using Customers.Models.BetList;
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

            Tuple<int, IEnumerable<AbnormalBetTicket>> abnormalBetTicketData;
            int totalSportsbookBetTickets;

            if (SiteTypeConst.IsCN88(siteType))
            {
                abnormalBetTicketData = service.GetAbnormalBetTickets(custId, fromDate, toDate, pageIndex, searchViewModel.PageSize);
                totalSportsbookBetTickets = service.GetTotalSporksbookBetTickets(custId, fromDate, toDate);
            }
            else
            {
                abnormalBetTicketData= service.GetAbnormalBetTicketsCreditLicensee(custId, fromDate, toDate, pageIndex, searchViewModel.PageSize);
                totalSportsbookBetTickets = service.GetTotalSporksbookBetTicketsCreditLicensee(custId, fromDate, toDate);
            }

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
                TotalSportsbookBetTickets = totalSportsbookBetTickets
            };

            return View("~/Areas/Customers/Views/BetList/AbnormalBetTicket.cshtml", vm);
        }

        public ActionResult GetAbnormalBetTicketPaging(AbnormalBetTicketViewModel viewModel)
        {
            if (HasReadManageProblem(viewModel.SiteType))
            {
                var abnormalBetTickets = SiteTypeConst.IsCN88(viewModel.SiteType)
                    ? service.GetAbnormalBetTickets(
                        viewModel.CustId,
                        viewModel.FromDate,
                        viewModel.ToDate.GetEndOfDate(),
                        viewModel.PageIndex,
                        viewModel.PageSize)
                    : service.GetAbnormalBetTicketsCreditLicensee(
                        viewModel.CustId,
                        viewModel.FromDate,
                        viewModel.ToDate.GetEndOfDate(),
                        viewModel.PageIndex,
                        viewModel.PageSize);

                return JsonData(abnormalBetTickets);
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
