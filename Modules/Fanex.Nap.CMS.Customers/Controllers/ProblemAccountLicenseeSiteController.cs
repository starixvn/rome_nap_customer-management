namespace Fanex.Nap.CMS.Customers.Controllers
{
    using System.Web.Mvc;
    using Constants;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Infrastructure;
    using Infrastructure.Controllers;
    using Services;
    using ViewModels.ProblemAccountLicenseeSite;

    [NapSuperAuth]
    public class ProblemAccountLicenseeSiteController : BaseController
    {
        private readonly IProblemAccountLicenseeSiteService service;

        public ProblemAccountLicenseeSiteController()
            : this(null)
        {
        }

        public ProblemAccountLicenseeSiteController(IProblemAccountLicenseeSiteService service)
        {
            this.service = service ?? new ProblemAccountLicenseeSiteService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountLicenseeSites, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var searchFilter = (SearchLicenseeSiteViewModel)TempData[SearchFilterSessionKeyConst.Licensee];
            var vm = new IndexLicenseeSiteViewModel(
                HasRight(CMSFunctionKeys.ManageProblemAccountLicenseeSites, Constant.Policy.Host),
                searchFilter != null,
                searchFilter ?? new SearchLicenseeSiteViewModel(),
                service.GetReasons(),
                CurrentSession.Username,
                service.GetCurrencies(),
                service.GetPreventiveActions(),
                service.GetPreventiveActionReasons());

            return View("~/Areas/Customers/Views/ProblemAccountLicenseeSite/Index.cshtml", vm);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountLicenseeSites, Policy = Constant.Policy.Read)]
        public ActionResult Search(SearchLicenseeSiteViewModel vm)
            => JsonData(service.GetProblemAccounts(
                vm.From,
                vm.To,
                vm.Acknowledge,
                vm.VIP,
                vm.Currency,
                vm.IsReduced,
                vm.Reason,
                vm.PreventiveActionId,
                vm.Member,
                vm.Licensee,
                vm.PageIndex,
                vm.PageSize));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountLicenseeSites, Policy = Constant.Policy.Host)]
        public ActionResult MarkNew(MarkNewLicenseeSiteViewModel vm)
        {
            if (vm.IsInvalid())
            {
                return BadRequest();
            }

            var result = service.MarkNew(
                vm.Usernames,
                vm.PreventiveAction,
                vm.Reason,
                vm.Comment,
                CurrentSession.UserId,
                CurrentContext.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountLicenseeSites, Policy = Constant.Policy.Host)]
        public ActionResult Update(MarkNewLicenseeSiteViewModel vm)
        {
            if (vm.IsInvalid())
            {
                return BadRequest();
            }

            var result = service.Update(
                vm.Usernames,
                vm.PreventiveAction,
                vm.Reason,
                vm.Comment,
                CurrentSession.UserId,
                CurrentContext.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountLicenseeSites, Policy = Constant.Policy.Host)]
        public ActionResult Unmark(string usernames)
        {
            if (string.IsNullOrWhiteSpace(usernames))
            {
                return BadRequest();
            }

            var result = service.Unmark(usernames, CurrentSession.UserId, CurrentContext.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountLicenseeSites, Policy = Constant.Policy.Host)]
        public ActionResult ReduceBL(ReduceBLLicenseeSiteViewModel vm)
        {
            if (vm.IsInvalid())
            {
                return BadRequest();
            }

            var result = service.ReduceBL(
                vm.Usernames,
                vm.MaxPerMatchValue,
                vm.MaxBetValue,
                CurrentSession.UserId,
                CurrentContext.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountLicenseeSites, Policy = Constant.Policy.Host)]
        public ActionResult RemoveReduceBL(string usernames, bool? confirmed = null)
        {
            if (string.IsNullOrWhiteSpace(usernames))
            {
                return BadRequest();
            }

            var result = service.RemoveReduceBL(usernames, confirmed ?? false, CurrentSession.UserId, CurrentContext.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountLicenseeSites, Policy = Constant.Policy.Host)]
        public ActionResult UpdateInformingStatus(string usernames, byte informedType)
        {
            if (string.IsNullOrWhiteSpace(usernames))
            {
                return BadRequest();
            }

            var result = service.UpdateInformingStatus(usernames, informedType, CurrentSession.UserId, CurrentContext.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }
    }
}
