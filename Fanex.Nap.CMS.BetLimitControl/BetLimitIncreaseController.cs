namespace Fanex.NAP.CMS.BetLimitControl
{
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
    using Fanex.NAP.CMS.BetLimitControl.Models.BetLimitIncrease;
    using Fanex.NAP.CMS.BetLimitControl.Request.BetLimitIncrease;
    using Fanex.NAP.CMS.BetLimitControl.Serivces;
    using Fanex.NAP.CMS.BetLimitControl.ViewModels;
    using Fanex.NAP.Client.Helpers.Business;
    using Fanex.NAP.Client.Attributes;
    using System.Web.Mvc;

    public class BetLimitIncreaseController : BaseController
    {
        private readonly ISettingService settingService;
        private readonly IBetLimitIncreaseService betLimitIncreaseService;

        public BetLimitIncreaseController() : this(new SettingService(), new BetLimitIncreaseService())
        {
        }

        public BetLimitIncreaseController(ISettingService settingService, IBetLimitIncreaseService betLimitIncreaseService)
        {
            this.settingService = settingService;
            this.betLimitIncreaseService = betLimitIncreaseService;
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.BetLimitIncrease, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var viewModel = new BetLimitIncreaseViewModel(
                settingService.GetAllSites(),
                settingService.GetAllCurrencies(),
                HasRight(CMSFunctionKeys.BetLimitIncrease, Constant.Policy.Host));

            return View("~/Areas/BetLimitIncrease/Views/Index.cshtml", viewModel);
        }
        
        [NapAuth(FunctionKey = CMSFunctionKeys.BetLimitIncrease, Policy = Constant.Policy.Read)]
        public ActionResult Search(SearchBetLimitIncreaseRequest request)
            => JsonData(betLimitIncreaseService.SearchBetLimitIncrease(request));
        
        [NapAuth(FunctionKey = CMSFunctionKeys.BetLimitIncrease, Policy = Constant.Policy.Host)]
        public ActionResult Add(AddBetLimitIncreaseRequest request)
            => JsonData(betLimitIncreaseService.AddBetLimitIncrease(request, CurrentContext.IP, CurrentSession.UserId));
        
        [NapAuth(FunctionKey = CMSFunctionKeys.BetLimitIncrease, Policy = Constant.Policy.Host)]
        public ActionResult Update(BetLimitIncreaseModel request)
            => JsonData(betLimitIncreaseService.UpdateBetLimitIncrease(request, CurrentContext.IP, CurrentSession.UserId));
        
        [NapAuth(FunctionKey = CMSFunctionKeys.BetLimitIncrease, Policy = Constant.Policy.Host)]
        public ActionResult Delete(BetLimitIncreaseModel request)
            => JsonData(betLimitIncreaseService.DeleteBetLimitIncrease(request, CurrentContext.IP, CurrentSession.UserId));
    }
}
