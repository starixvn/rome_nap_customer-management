namespace Fanex.Nap.CMS.Customers.Controllers
{
    using Fanex.Nap.CMS.Customers.Services.ManageFollowBetAccount;
    using Fanex.Nap.CMS.Customers.ViewModels.ManageFollowBetAccount;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using System.Web.Mvc;

    public class ManageFollowBetAccountController : BaseController
    {
        private readonly IManageFollowBetAccountService service;

        public ManageFollowBetAccountController() : this(null)
        {
        }

        internal ManageFollowBetAccountController(IManageFollowBetAccountService service)
        {
            this.service = service ?? new ManageFollowBetAccountService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageFollowBetAccount, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var viewModel = new ManageFollowBetAccountViewModel(
                HasRight(CMSFunctionKeys.ManageFollowBetAccount, Constant.Policy.Host),
                service.GetSites(),
                service.GetCurrencies(),
                service.GetDummyAgents());
            return View("~/Areas/Customers/Views/ManageFollowBetAccount/Index.cshtml", viewModel);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ManageFollowBetAccount, Policy = Constant.Policy.Read)]
        public ActionResult Search(SearchFollowBetAccountViewModel viewModel)
            => JsonData(service.GetFollowBetAccountList(viewModel));
        
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ManageFollowBetAccount, Policy = Constant.Policy.Host)]
        public ActionResult AddNew(AddNewFollowBetAccountViewModel viewModel)
        {
            viewModel.UserId = CurrentSession.UserId;
            viewModel.IP = CurrentContext.IP;
            return JsonData(service.AddNewFollowBetAccount(viewModel), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ManageFollowBetAccount, Policy = Constant.Policy.Host)]
        public ActionResult Update(UpdateFollowBetAccountViewModel viewModel)
        {
            viewModel.UserId = CurrentSession.UserId;
            viewModel.IP = CurrentContext.IP;
            return JsonData(service.UpdateFollowBetAccount(viewModel), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ManageFollowBetAccount, Policy = Constant.Policy.Host)]
        public ActionResult Remove(RemoveFollowBetAccountViewModel viewModel)
        {
            viewModel.UserId = CurrentSession.UserId;
            viewModel.IP = CurrentContext.IP;
            return JsonData(service.RemoveFollowBetAccount(viewModel), JsonRequestBehavior.DenyGet);
        }
    }
}
