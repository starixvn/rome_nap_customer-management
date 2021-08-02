namespace Fanex.Nap.CMS.BetRestriction.Controllers
{
    using Fanex.Nap.CMS.BetRestriction.Criterias;
    using Fanex.Nap.CMS.BetRestriction.Models.Basketball;
    using Fanex.Nap.CMS.BetRestriction.Requests;
    using Fanex.Nap.CMS.BetRestriction.Services;
    using Fanex.Nap.CMS.BetRestriction.ViewModels.Basketball;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using System.Web.Mvc;

    public class BetRestrictionBasketballController : BaseController
    {
        private readonly IBetRestrictionBasketballService _betRestrictionBasketballService;

        private readonly string _indexPath = "~/Areas/BetRestrictionBasketball/Index.cshtml";

        public BetRestrictionBasketballController() : this(null)
        { 
        }

        public BetRestrictionBasketballController(IBetRestrictionBasketballService betRestrictionBasketballService)
        {
            _betRestrictionBasketballService = betRestrictionBasketballService ?? new BetRestrictionBasketballService();
        }

        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_BasketBall, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var viewModel = new BetRestrictionBasketballViewModel
            {
                Sites = _betRestrictionBasketballService.GetSites(new GetAllSiteCriteria()),
                Currency = _betRestrictionBasketballService.GetCurrency(new GetAllCurrencyCriteria())
            };
            return View(_indexPath, viewModel);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_BasketBall, Policy = Constant.Policy.Read)]
        public JsonResult Search(SearchRequest request)
            => Json(_betRestrictionBasketballService.Search(CreateSearchCriteriaFromRequest(request)));

        public JsonResult SearchAccountPaging(SearchRequest request)
            => Json(_betRestrictionBasketballService.SearchDataGrid<AccountModel>(CreateSearchCriteriaFromRequest(request)));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_BasketBall, Policy = Constant.Policy.Read)]
        public JsonResult CheckAddAccount(AccountRequest request) => Json(_betRestrictionBasketballService.CheckAddAccount(request));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_BasketBall, Policy = Constant.Policy.Read)]
        public JsonResult CheckEditAccount(AccountRequest request) => Json(_betRestrictionBasketballService.CheckEditAccount(request));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_BasketBall, Policy = Constant.Policy.Host)]
        public JsonResult AddAccount(AccountRequest request) => Json(_betRestrictionBasketballService.ExecuteNonQuery(new AddAccountCriteria
        {
            UserName = request.UserName,
            LiveBetAccept = request.LiveBetAccept,
            SeizeBetTicket = request.SeizeBetTicket,
            FollowAccount = request.FollowAccount,
            UserId = CurrentSession.UserId,
            IP = CurrentContext.IP,
            SportType = 2
        }));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_BasketBall, Policy = Constant.Policy.Read)]
        public JsonResult GetAccountDetail(int custId) => Json(_betRestrictionBasketballService.GetAccountDetailForEdit(custId));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_BasketBall, Policy = Constant.Policy.Host)]
        public JsonResult EditAccount(AccountRequest request) => Json(_betRestrictionBasketballService.ExecuteNonQuery(new EditAccountCriteria
        {
            CustId = request.CustId,
            LiveBetAccept = request.LiveBetAccept,
            SeizeBetTicket = request.SeizeBetTicket,
            FollowAccount = request.FollowAccount,
            UserId = CurrentSession.UserId,
            IP = CurrentContext.IP
        }));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_BasketBall, Policy = Constant.Policy.Host)]
        public JsonResult RemoveAccount(int custId) => Json(_betRestrictionBasketballService.ExecuteNonQuery(new RemoveAccountCriteria
        {
            CustId = custId,
            UserId = CurrentSession.UserId,
            IP = CurrentContext.IP
        }));

        private SearchAccountCriteria CreateSearchCriteriaFromRequest(SearchRequest request)
            => new SearchAccountCriteria
            {
                UserName = request.UserName,
                RoleId = request.RoleId,
                SiteId = request.SiteId,
                CurrencyIds = request.CurrencyIds,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };
    }
}
