namespace Fanex.Nap.CMS.BetRestriction.Controllers
{
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.BetRestriction.Criterias;
    using Fanex.NAP.CMS.Services.Implements;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.Nap.CMS.BetRestriction.ViewModels;
    using Fanex.NAP.Client.Helpers.Business;
    using Fanex.NAP.Client.Attributes;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.BetRestriction.Requests;
    using Fanex.Nap.CMS.BetRestriction.Models;

    public class BetRestrictionBadmintonController : BaseController
    {
        private readonly IBetRestrictionService _betRestrictionService;

        private const string _indexPath = "~/Areas/BetRestriction/Views/Badminton/Index.cshtml";

        private const short _sportType = 9;

        public BetRestrictionBadmintonController() : this(null)
        { 
        }

        public BetRestrictionBadmintonController(IBetRestrictionService betRestrictionService)
        {
            _betRestrictionService = betRestrictionService ?? new BetRestrictionService();
        }

        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_Badminton, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var viewModel = new BetRestrictionViewModel
            {
                Sites = _betRestrictionService.GetSites(new GetAllSiteCriteria()),
                Currency = _betRestrictionService.GetCurrency(new GetAllCurrencyCriteria()),
                HasHostPermission = HasRight(CMSFunctionKeys.BetRestriction_Badminton, Constant.Policy.Host)
            };
            return View(_indexPath, viewModel);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_Badminton, Policy = Constant.Policy.Read)]
        public JsonResult Search(SearchRequest request)
            => Json(_betRestrictionService.Search(CreateSearchCriteriaFromRequest(request)));

        public JsonResult SearchAccountPaging(SearchRequest request)
            => Json(_betRestrictionService.SearchDataGrid<AccountModel>(CreateSearchCriteriaFromRequest(request)));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_Badminton, Policy = Constant.Policy.Host)]
        public JsonResult CheckAddAccount(AccountRequest request) => Json(_betRestrictionService.CheckAddAccount(request, _sportType));
        
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_Badminton, Policy = Constant.Policy.Host)]
        public JsonResult AddAccount(AccountRequest request) => Json(_betRestrictionService.ExecuteNonQuery(new AddAccountCriteria
        {
            UserName = request.UserName,
            LiveBetAccept = request.LiveBetAccept,
            SeizeBetTicket = request.SeizeBetTicket,
            FollowAccount = request.FollowAccount,
            AccumulateBalance = request.AccumulateBalance,
            SportType = _sportType,
            UserId = CurrentSession.UserId,
            IP = CurrentContext.IP
        }));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_Badminton, Policy = Constant.Policy.Read)]
        public JsonResult GetAccountDetail(int custId) => Json(_betRestrictionService.GetAccountDetailForEdit(custId, _sportType));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_Badminton, Policy = Constant.Policy.Host)]
        public JsonResult EditAccount(AccountRequest request) => Json(_betRestrictionService.ExecuteNonQuery(new EditAccountCriteria
        {
            CustId = request.CustId,
            LiveBetAccept = request.LiveBetAccept,
            SeizeBetTicket = request.SeizeBetTicket,
            FollowAccount = request.FollowAccount,
            AccumulateBalance = request.AccumulateBalance,
            SportType = _sportType,
            UserId = CurrentSession.UserId,
            IP = CurrentContext.IP
        }));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_Badminton, Policy = Constant.Policy.Host)]
        public JsonResult RemoveAccount(int custId) => Json(_betRestrictionService.ExecuteNonQuery(new RemoveAccountCriteria
        {
            CustId = custId,
            SportType = _sportType,
            UserId = CurrentSession.UserId,
            IP = CurrentContext.IP
        }));

        private SearchAccountCriteria CreateSearchCriteriaFromRequest(SearchRequest request)
            => new SearchAccountCriteria
            {
                UserName = request.UserName,
                RoleId = request.RoleId,
                SiteId = request.SiteId,
                SportType = _sportType,
                CurrencyIds = request.CurrencyIds,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
    }
}
