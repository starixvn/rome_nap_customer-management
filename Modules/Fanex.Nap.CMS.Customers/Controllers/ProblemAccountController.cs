namespace Fanex.Nap.CMS.Customers.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using Infrastructure;
    using Infrastructure.Controllers;
    using Infrastructure.Models;
    using Infrastructure.Services.Interfaces;
    using NAP.Core.Business;
    using NAP.Core.Web.Attribute;
    using Resources;
    using Services;
    using ViewModels.ProblemAccount;

    public class ProblemAccountController : BaseController
    {
        public const string SearchFilterSessionKey = "ProblemAccountSearchFilterSessionKey";

        private readonly IProblemAccountService service;

        public ProblemAccountController()
            : this(null)
        {
        }

        public ProblemAccountController(IProblemAccountService service)
        {
            this.service = service ?? new ProblemAccountService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCN88, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var vm = new IndexViewModel(
                HasRight(CMSFunctionKeys.ManageProblemAccountCN88, Constant.Policy.Host),
                (SearchViewModel)TempData[SearchFilterSessionKey],
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

            IExcelExporter exporter = new ProblemAccountExporter(vm, data, service.GetReasons(true));
            exporter.ResponseStream();
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCN88, Policy = Constant.Policy.Host)]
        public ActionResult MarkNew(MarkNewViewModel vm)
        {
            var result = vm.IsInvalid()
                ? BadRequestResult()
                : service.MarkNew(
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
            var result = vm.IsInvalid()
                ? BadRequestResult()
                : service.Unmark(
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
            var result = vm.IsInvalid()
                ? BadRequestResult()
                : service.ReduceBL(
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
            var result = vm.IsInvalid()
                ? BadRequestResult()
                : service.Inactivate(
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
            var result = vm.IsInvalid()
                ? BadRequestResult()
                : service.RemoveReduceBL(
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
            var result = vm.IsInvalid()
                ? BadRequestResult()
                : service.UpdateInformingStatus(
                    vm.Usernames,
                    vm.Level,
                    vm.InformedType,
                    vm.IsApplyDownlines,
                    CurrentSession.UserId,
                    CurrentContext.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        private static Result BadRequestResult()
            => new Result((int)HttpStatusCode.BadRequest, ProblemAccountResource.BadRequest);
    }
}
