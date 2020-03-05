namespace Fanex.Nap.CMS.Customers.Controllers
{
    using System.Web.Mvc;
    using Constants;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Infrastructure;
    using Infrastructure.Controllers;
    using Infrastructure.Services.Interfaces;
    using Services;
    using ViewModels.ProblemAccount;
    using ViewModels.ProblemAccountCreditSite;

    [NapSuperAuth]
    public class ProblemAccountCreditSiteController : BaseController
    {
        private readonly IProblemAccountCreditSiteService service;

        public ProblemAccountCreditSiteController()
            : this(null)
        {
        }

        public ProblemAccountCreditSiteController(IProblemAccountCreditSiteService service)
        {
            this.service = service ?? new ProblemAccountCreditSiteService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCreditSites, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var searchFilter = (SearchCreditSiteViewModel)TempData[SearchFilterSessionKeyConst.Credit];
            var vm = new IndexCreditSiteViewModel(
                HasRight(CMSFunctionKeys.ManageProblemAccountCreditSites, Constant.Policy.Host),
                searchFilter != null,
                searchFilter ?? new SearchCreditSiteViewModel(),
                service.GetReasons(true),
                service.GetReasons(false),
                service.GetCurrencies(),
                service.GetPreventiveActions());

            return View("~/Areas/Customers/Views/ProblemAccountCreditSite/Index.cshtml", vm);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCreditSites, Policy = Constant.Policy.Read)]
        public ActionResult Search(SearchCreditSiteViewModel vm)
            => JsonData(service.GetProblemAccounts(
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
                vm.PreventiveActionId,
                vm.PageIndex,
                vm.PageSize));

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCreditSites, Policy = Constant.Policy.Read)]
        public void Export(SearchCreditSiteViewModel vm)
        {
            var data = service.GetProblemAccounts(
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
                vm.PreventiveActionId,
                1,
                int.MaxValue - 1);

            IExcelExporter exporter = new ProblemAccountCreditSitesExporter(
                vm,
                data,
                service.GetReasons(true),
                service.GetCurrencies(),
                service.GetPreventiveActions());

            exporter.ResponseStream();
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCreditSites, Policy = Constant.Policy.Host)]
        public ActionResult MarkNew(MarkNewCreditSiteViewModel vm)
        {
            if (vm.IsInvalid())
            {
                return BadRequest();
            }

            var result = service.MarkNew(
                vm.Usernames,
                vm.Level,
                vm.PreventiveAction,
                vm.Reason,
                vm.Comment,
                CurrentSession.UserId,
                CurrentContext.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCreditSites, Policy = Constant.Policy.Host)]
        public ActionResult Update(MarkNewCreditSiteViewModel vm)
        {
            if (vm.IsInvalid())
            {
                return BadRequest();
            }

            var result = service.Update(
                vm.Usernames,
                vm.Level,
                vm.PreventiveAction,
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

            var result = service.Unmark(
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

            var result = service.ReduceBL(
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

            var result = service.RemoveReduceBL(
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
