namespace Fanex.Nap.CMS.BetRestriction.Controllers
{
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.BetRestriction.Criterias;
    using Fanex.NAP.CMS.Services.Implements;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.Nap.CMS.BetRestriction.ViewModels;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.BetRestriction.Requests;
    using Fanex.Nap.CMS.BetRestriction.Models;
    using Fanex.Nap.CMS.Infrastructure.Web;

    public class BetRestrictionSoccerController : BaseController
    {
        private readonly IBetRestrictionService _betRestrictionService;

        private const string _indexPath = "~/Areas/BetRestriction/Views/Soccer/Index.cshtml";

        private const short _sportType = 1;

        public BetRestrictionSoccerController() : this(null)
        {
        }

        public BetRestrictionSoccerController(IBetRestrictionService betRestrictionService)
        {
            _betRestrictionService = betRestrictionService ?? new BetRestrictionService();
        }

        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_Soccer, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var viewModel = new BetRestrictionViewModel
            {
                Sites = _betRestrictionService.GetSites(new GetAllSiteCriteria()),
                Currency = _betRestrictionService.GetCurrency(new GetAllCurrencyCriteria()),
                LeagueGroups = _betRestrictionService.GetLeagueGroup(new GetAllLeagueGroupCriteria(_sportType))
            };

            return View(_indexPath, viewModel);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_Soccer, Policy = Constant.Policy.Read)]
        public JsonResult Search(SearchRequest request)
            => Json(_betRestrictionService.Search(CreateSearchCriteriaFromRequest(request)));

        public JsonResult SearchAccountPaging(SearchRequest request)
            => Json(_betRestrictionService.SearchDataGrid<AccountModel>(CreateSearchCriteriaFromRequest(request)));

        public JsonResult SearchSitePaging(SearchRequest request)
            => Json(_betRestrictionService.SearchDataGrid<SiteModel>(new SearchSiteCriteria(request.PageSize, request.PageIndex)));

        public JsonResult SearchCurrencyPaging(SearchRequest request)
            => Json(_betRestrictionService.SearchDataGrid<CurrencyModel>(new SearchCurrencyCriteria(request.PageSize, request.PageIndex)));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_Soccer, Policy = Constant.Policy.Host)]
        public JsonResult CheckAddAccount(AccountRequest request) => Json(_betRestrictionService.CheckAddAccount(request, _sportType));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_Soccer, Policy = Constant.Policy.Host)]
        public JsonResult AddAccount(AccountRequest request) => Json(_betRestrictionService.ExecuteNonQuery(new AddAccountCriteria
        {
            UserName = request.UserName,
            LiveBetAccept = request.LiveBetAccept,
            MaxBet = request.MaxBet,
            MaxPerMatch = request.MaxPerMatch,
            SeizeBetTicket = request.SeizeBetTicket,
            FollowAccount = request.FollowAccount,
            SportType = _sportType,
            UserId = CurrentSession.UserId,
            IP = CurrentContext.IP
        }));


        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_Soccer, Policy = Constant.Policy.Read)]
        public JsonResult GetAccountDetail(int custId) => Json(_betRestrictionService.GetAccountDetailForEdit(custId, _sportType));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_Soccer, Policy = Constant.Policy.Host)]
        public JsonResult EditAccount(AccountRequest request) => Json(_betRestrictionService.ExecuteNonQuery(new EditAccountCriteria
        {
            CustId = request.CustId,
            LiveBetAccept = request.LiveBetAccept,
            MaxBet = request.MaxBet,
            MaxPerMatch = request.MaxPerMatch,
            SeizeBetTicket = request.SeizeBetTicket,
            FollowAccount = request.FollowAccount,
            SportType = _sportType,
            UserId = CurrentSession.UserId,
            IP = CurrentContext.IP
        }));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_Soccer, Policy = Constant.Policy.Host)]
        public JsonResult RemoveAccount(int custId) => Json(_betRestrictionService.ExecuteNonQuery(new RemoveAccountCriteria
        {
            CustId = custId,
            SportType = _sportType,
            UserId = CurrentSession.UserId,
            IP = CurrentContext.IP
        }));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_Soccer, Policy = Constant.Policy.Host)]
        public JsonResult AddSite(SiteRequest request) => Json(_betRestrictionService.ExecuteNonQuery(new AddSiteCriteria
        {
            SiteId = request.SiteId,
            UserId = CurrentSession.UserId,
            LiveBetAccept = request.LiveBetAccept,
            SeizeBetTicket = request.SeizeBetTicket,
            FollowAccount = request.FollowAccount,
            IP = CurrentContext.IP
        }));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_Soccer, Policy = Constant.Policy.Read)]
        public JsonResult GetSiteDetail(int siteId) => Json(_betRestrictionService.GetSiteDetailForEdit(siteId));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_Soccer, Policy = Constant.Policy.Read)]
        public JsonResult GetCurrencyDetail(int exchangeId, int type) => Json(_betRestrictionService.GetCurrencyDetailForEdit(exchangeId, type));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_Soccer, Policy = Constant.Policy.Host)]
        public JsonResult EditSite(SiteRequest request) => Json(_betRestrictionService.ExecuteNonQuery(new EditSiteCriteria
        {
            SiteId = request.SiteId,
            UserId = CurrentSession.UserId,
            LiveBetAccept = request.LiveBetAccept,
            SeizeBetTicket = request.SeizeBetTicket,
            FollowAccount = request.FollowAccount,
            IP = CurrentContext.IP
        }));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_Soccer, Policy = Constant.Policy.Host)]
        public JsonResult RemoveSite(int siteId) => Json(_betRestrictionService.ExecuteNonQuery(new RemoveSiteCriteria
        {
            SiteId = siteId,
            UserId = AppSession.CurrentUser.Id,
            IP = CurrentContext.IP
        }));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_Soccer, Policy = Constant.Policy.Host)]
        public JsonResult AddCurrency(CurrencyRequest request) => Json(_betRestrictionService.ExecuteNonQuery(new AddCurrencyCriteria
        {
            ExchangeId = request.ExchangeId,
            LiveBetAccept = request.LiveBetAccept,
            SeizeBetTicket = request.SeizeBetTicket,
            FollowAccount = request.FollowAccount,
            Type = request.Type,
            IP = CurrentContext.IP,
            UserId = CurrentSession.UserId
        }));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_Soccer, Policy = Constant.Policy.Host)]
        public JsonResult EditCurrency(CurrencyRequest request) => Json(_betRestrictionService.ExecuteNonQuery(new EditCurrencyCriteria
        {
            ExchangeId = request.ExchangeId,
            LiveBetAccept = request.LiveBetAccept,
            SeizeBetTicket = request.SeizeBetTicket,
            FollowAccount = request.FollowAccount,
            CurrentType = request.Type,
            NewType = request.NewType,
            IP = CurrentContext.IP,
            UserId = CurrentSession.UserId
        }));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_Soccer, Policy = Constant.Policy.Host)]
        public JsonResult RemoveCurrency(CurrencyRequest request) => Json(_betRestrictionService.ExecuteNonQuery(new RemoveCurrencyCriteria
        {
            ExchangeId = request.ExchangeId,
            Type = request.Type,
            IP = CurrentContext.IP,
            UserId = CurrentSession.UserId
        }));

        private SearchAccountCriteria CreateSearchCriteriaFromRequest(SearchRequest request)
            => new SearchAccountCriteria
            {
                UserName = request.UserName,
                RoleId = request.RoleId,
                Status = request.Status,
                SiteId = request.SiteId,
                CurrencyIds = request.CurrencyIds,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                SportType = _sportType
            };
    }
}
