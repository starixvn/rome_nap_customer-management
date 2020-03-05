namespace Fanex.Nap.CMS.Customers.Controllers
{
    using System.Web.Mvc;
    using Constants;
    using Infrastructure;
    using Infrastructure.Controllers;
    using Infrastructure.Services.Interfaces;
    using NAP.Core.Business;
    using NAP.Core.Web.Attribute;
    using Services;
    using ViewModels.ProblemAccount;

    [NapSuperAuth]
    public class ProblemAccountController : BaseController
    {
        private readonly IProblemAccountCN88Service service;

        public ProblemAccountController()
            : this(null)
        {
        }

        public ProblemAccountController(IProblemAccountCN88Service service)
        {
            this.service = service ?? new ProblemAccountCN88Service();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCN88, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var searchFilter = (SearchViewModel)TempData[SearchFilterSessionKeyConst.CN88];
            var vm = new IndexViewModel(
                HasRight(CMSFunctionKeys.ManageProblemAccountCN88, Constant.Policy.Host),
                searchFilter != null,
                searchFilter ?? new SearchViewModel { Currency = CurrencyConst.CY },
                service.GetReasons(true),
                service.GetReasons(false),
                CurrentSession.Username);

            return View("~/Areas/Customers/Views/ProblemAccount/Index.cshtml", vm);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCN88, Policy = Constant.Policy.Read)]
        public ActionResult Search(SearchViewModel vm)
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
                vm.PageIndex,
                vm.PageSize));

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCN88, Policy = Constant.Policy.Read)]
        public void Export(SearchViewModel vm)
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
                1,
                int.MaxValue - 1);

            IExcelExporter exporter = new ProblemAccountCN88Exporter(vm, data, service.GetReasons(true));
            exporter.ResponseStream();
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCN88, Policy = Constant.Policy.Host)]
        public ActionResult MarkNew(MarkNewViewModel vm)
        {
            if (vm.IsInvalid())
            {
                return BadRequest();
            }

            var result = service.MarkNew(
                vm.Usernames,
                vm.Level,
                vm.Reason,
                vm.Comment,
                CurrentSession.UserId,
                CurrentContext.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCN88, Policy = Constant.Policy.Host)]
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

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCN88, Policy = Constant.Policy.Host)]
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

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCN88, Policy = Constant.Policy.Host)]
        public ActionResult Inactivate(InactivateViewModel vm)
        {
            if (vm.IsInvalid())
            {
                return BadRequest();
            }

            var result = service.Inactivate(
                vm.Usernames,
                vm.Level,
                vm.Confirmed ?? false,
                CurrentSession.UserId,
                CurrentContext.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCN88, Policy = Constant.Policy.Host)]
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

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCN88, Policy = Constant.Policy.Host)]
        public ActionResult UpdateInformingStatus(UpdateInformingStatusViewModel vm)
        {
            if (vm.IsInvalid())
            {
                return BadRequest();
            }

            var result = service.UpdateInformingStatus(
                vm.Usernames,
                vm.Level,
                vm.InformedType,
                vm.IsApplyDownlines,
                CurrentSession.UserId,
                CurrentContext.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }
    }
}
