namespace Fanex.Nap.CMS.Customers.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using Infrastructure;
    using Infrastructure.Controllers;
    using Infrastructure.Models;
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
            //var reasons = service.GetReasons();
            var vm = new IndexViewModel(
                HasRight(CMSFunctionKeys.ManageProblemAccountCN88, Constant.Policy.Host),
                (SearchViewModel)TempData[SearchFilterSessionKey]);

            return View("~/Areas/Customers/Views/ProblemAccount/Index.cshtml", vm);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCN88, Policy = Constant.Policy.Read)]
        public ActionResult Search(SearchViewModel vm)
        {
            return JsonData("");
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCN88, Policy = Constant.Policy.Host)]
        public ActionResult MarkNew(MarkNewViewModel vm)
        {
            UpdateUserIdAndIP(vm);
            vm.Comment = HtmlEncodeText(vm.Comment);
            var result = vm.IsInvalid()
                ? BadRequestResult()
                : service.MarkNew(vm.Usernames, vm.Level, vm.Reason, vm.Comment, vm.UserId, vm.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCN88, Policy = Constant.Policy.Host)]
        public ActionResult Unmark(UnmarkViewModel vm)
        {
            UpdateUserIdAndIP(vm);
            var result = vm.IsInvalid()
                ? BadRequestResult()
                : service.Unmark(vm.Usernames, vm.Level, vm.AbnormalBet, vm.UnmarkByOPS, vm.UserId, vm.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCN88, Policy = Constant.Policy.Host)]
        public ActionResult ReduceBL(ReduceBLViewModel vm)
        {
            UpdateUserIdAndIP(vm);
            var result = vm.IsInvalid()
                ? BadRequestResult()
                : service.ReduceBL(vm.Usernames, vm.Level, vm.MaxPerEventValue, vm.MaxPerBetValue, vm.UserId, vm.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCN88, Policy = Constant.Policy.Host)]
        public ActionResult Inactivate(ActionViewModel vm)
        {
            UpdateUserIdAndIP(vm);
            var result = vm.IsInvalid()
                ? BadRequestResult()
                : service.Inactivate(vm.Usernames, vm.Level, vm.UserId, vm.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageProblemAccountCN88, Policy = Constant.Policy.Host)]
        public ActionResult RemoveReduceBL(UnmarkViewModel vm)
        {
            UpdateUserIdAndIP(vm);
            var result = vm.IsInvalid()
                ? BadRequestResult()
                : service.RemoveReduceBL(vm.Usernames, vm.Level, vm.AbnormalBet, vm.UnmarkByOPS, vm.UserId, vm.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        private void UpdateUserIdAndIP(ActionViewModel vm)
        {
            vm.UserId = CurrentSession.UserId;
            vm.IP = CurrentContext.IP;
        }

        private static string HtmlEncodeText(string text)
            => string.IsNullOrWhiteSpace(text) ? text : WebUtility.HtmlEncode(text);

        private static Result BadRequestResult()
            => new Result((int)HttpStatusCode.BadRequest, ProblemAccountResource.BadRequest);
    }
}
