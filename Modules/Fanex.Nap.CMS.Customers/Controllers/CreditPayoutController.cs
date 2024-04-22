namespace Fanex.Nap.CMS.Customers.Controllers
{
    using Fanex.Nap.CMS.Customers.Criteria.CreditPayout;
    using Fanex.Nap.CMS.Customers.Services.CreditPayout;
    using Fanex.Nap.CMS.Customers.ViewModels.CreditPayout;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using System.Web.Mvc;

    public class CreditPayoutController : BaseController
    {
        private readonly ICreditPayoutService creditPayoutService;

        public CreditPayoutController() : this(null)
        {

        }
        
        public CreditPayoutController(ICreditPayoutService creditPayoutService)
        {
            this.creditPayoutService = creditPayoutService ?? new CreditPayoutService();
        }

        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.CreditPayoutManagement, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var viewModel = new CreditPaypoutViewModel(HasRight(CMSFunctionKeys.CreditPayoutManagement, Constant.Policy.Host));

            return View("~/Areas/Customers/Views/CreditPayout/Index.cshtml", viewModel);
        }

        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.CreditPayoutManagement, Policy = Constant.Policy.Read)]
        public ActionResult Match(int typeId)
        {
            return JsonData(creditPayoutService.GetMatches(typeId));
        }

        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.CreditPayoutManagement, Policy = Constant.Policy.Read)]
        public ActionResult Search(SelPayoutPromotionCriteria request)
        {
            var result = creditPayoutService.SearchPayout(request);

            return JsonData(result);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.CreditPayoutManagement, Policy = Constant.Policy.Host)]
        public ActionResult ValidateVerifyCode(VerifySelPayoutPromotionCriteria request)
        {
            var result = creditPayoutService.ValidateVerifyCode(request);

            if (request.ErrCode != 0)
            {
                return JsonData(new Result(request.ErrCode, request.ErrMsg));
            }

            return JsonData(result);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.CreditPayoutManagement, Policy = Constant.Policy.Host)]
        public ActionResult VerifyCode(UpdPayoutPromotionCriteria request)
        {
            request.UserId = CurrentSession.UserId;
            request.IP = CurrentContext.IP;
            var result = creditPayoutService.VerifyPayoutPromotion(request);

            return JsonData(result);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.CreditPayoutManagement, Policy = Constant.Policy.Host)]
        public ActionResult Update(UpdPayoutPromotionCriteria request)
        {
            request.UserId = CurrentSession.UserId;
            request.IP = CurrentContext.IP;
            var result = creditPayoutService.UpdPayoutPromotion(request);

            return JsonData(result);
        }
    }
}
