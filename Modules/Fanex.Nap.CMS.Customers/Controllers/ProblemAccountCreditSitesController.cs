namespace Fanex.Nap.CMS.Customers.Controllers
{
    using System;
    using System.Web.Mvc;
    using Constants;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Infrastructure;
    using Infrastructure.Controllers;
    using Infrastructure.Services.Interfaces;
    using Services;
    using ViewModels.ProblemAccount;

    public class ProblemAccountCreditSitesController : BaseController
    {
        // The release date of this feature.
        private static readonly DateTime DefaultMinDate = new DateTime(2020, 2, 1);

        private readonly IProblemAccountService service;

        public ProblemAccountCreditSitesController()
            : this(null)
        {
        }

        public ProblemAccountCreditSitesController(IProblemAccountService service)
        {
            this.service = service ?? new ProblemAccountService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCreditSites, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var searchFilter = (SearchViewModel)TempData[SearchFilterSessionKeyConst.Credit];
            var vm = new IndexViewModel(
                HasRight(CMSFunctionKeys.ManageProblemAccountCreditSites, Constant.Policy.Host),
                searchFilter != null,
                searchFilter ?? new SearchViewModel(),
                service.GetReasonsCreditSites(true),
                service.GetReasonsCreditSites(false),
                CurrentSession.Username,
                DefaultMinDate,
                service.GetCurrencies(),
                AccountStatusCreditSitesConst.AccountStatusListForFilter);

            return View("~/Areas/Customers/Views/ProblemAccountCreditSites/Index.cshtml", vm);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCreditSites, Policy = Constant.Policy.Read)]
        public ActionResult Search(SearchViewModel vm)
            => JsonData(service.GetProblemAccountsCreditSites(
                vm.From,
                vm.To,
                vm.Level,
                vm.Currency,
                vm.Status,
                vm.IsReduced,
                vm.Reason,
                vm.Username,
                vm.IsSearchDownlines,
                vm.OrderBy,
                vm.IsDirectlyMarked,
                vm.PageIndex,
                vm.PageSize));

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCreditSites, Policy = Constant.Policy.Read)]
        public void Export(SearchViewModel vm)
        {
            var data = service.GetProblemAccountsCreditSites(
                vm.From,
                vm.To,
                vm.Level,
                vm.Currency,
                vm.Status,
                vm.IsReduced,
                vm.Reason,
                vm.Username,
                vm.IsSearchDownlines,
                vm.OrderBy,
                vm.IsDirectlyMarked,
                1,
                int.MaxValue - 1);

            IExcelExporter exporter = new ProblemAccountCreditSitesExporter(
                vm,
                data,
                service.GetReasonsCreditSites(true),
                service.GetCurrencies());

            exporter.ResponseStream();
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCreditSites, Policy = Constant.Policy.Host)]
        public ActionResult MarkNew(MarkNewViewModel vm)
        {
            if (vm.IsInvalid())
            {
                return BadRequest();
            }

            var result = service.MarkNewCreditSites(
                vm.Usernames,
                vm.Level,
                vm.Reason,
                vm.Comment,
                CurrentSession.UserId,
                CurrentContext.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCreditSites, Policy = Constant.Policy.Host)]
        public ActionResult Unmark(UnmarkViewModel vm)
        {
            if (vm.IsInvalid())
            {
                return BadRequest();
            }

            var result = service.UnmarkCreditSites(
                vm.Usernames,
                vm.Level,
                vm.ExcludeAbnormalMember ?? false,
                vm.ExcludeDirectMark ?? false,
                CurrentSession.UserId,
                CurrentContext.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCreditSites, Policy = Constant.Policy.Host)]
        public ActionResult ReduceBL(ReduceBLViewModel vm)
        {
            if (vm.IsInvalid())
            {
                return BadRequest();
            }

            var result = service.ReduceBLCreditSites(
                vm.Usernames,
                vm.Level,
                vm.MaxPerEventValue,
                vm.MaxPerBetValue,
                CurrentSession.UserId,
                CurrentContext.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCreditSites, Policy = Constant.Policy.Host)]
        public ActionResult Close(InactivateViewModel vm)
        {
            if (vm.IsInvalid())
            {
                return BadRequest();
            }

            var result = service.Close(
                vm.Usernames,
                vm.Level,
                vm.Confirmed ?? false,
                CurrentSession.UserId,
                CurrentContext.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCreditSites, Policy = Constant.Policy.Host)]
        public ActionResult RemoveReduceBL(RemoveReduceBLViewModel vm)
        {
            if (vm.IsInvalid())
            {
                return BadRequest();
            }

            var result = service.RemoveReduceBLCreditSites(
                vm.Usernames,
                vm.Level,
                vm.IsAllDownlines ?? false,
                vm.Confirmed ?? false,
                CurrentSession.UserId,
                CurrentContext.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }
    }
}
