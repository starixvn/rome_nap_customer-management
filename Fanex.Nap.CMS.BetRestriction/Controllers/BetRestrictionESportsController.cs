namespace Fanex.Nap.CMS.BetRestriction.Controllers
{
    using Fanex.Nap.CMS.BetRestriction.Criterias;
    using Fanex.Nap.CMS.BetRestriction.Models;
    using Fanex.Nap.CMS.BetRestriction.Requests;
    using Fanex.Nap.CMS.BetRestriction.ViewModels;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.NAP.CMS.Services.Implements;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.Client.Helpers.Business;
    using Fanex.NAP.Client.Attributes;
    using System.Web.Mvc;

    public class BetRestrictionESportsController : BaseController
    {
        private readonly IBetRestrictionService _betRestrictionService;

        private const string _indexPath = "~/Areas/BetRestriction/Views/ESports/Index.cshtml";

        private const short _sportType = 43;

        public BetRestrictionESportsController() : this(null)
        {
        }

        public BetRestrictionESportsController(IBetRestrictionService betRestrictionService)
        {
            _betRestrictionService = betRestrictionService ?? new BetRestrictionService();
        }

        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_ESports, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var viewModel = new BetRestrictionViewModel
            {
                Sites = _betRestrictionService.GetSites(new GetAllSiteCriteria()),
                Currency = _betRestrictionService.GetCurrency(new GetAllCurrencyCriteria()),
                LeagueGroups = _betRestrictionService.GetLeagueGroup(new GetAllLeagueGroupCriteria(_sportType)),               
                HasHostPermission = HasRight(CMSFunctionKeys.BetRestriction_ESports, Constant.Policy.Host)
            };

            return View(_indexPath, viewModel);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_ESports, Policy = Constant.Policy.Read)]
        public JsonResult Search(SearchRequest request)
        {
            var data = _betRestrictionService.Search(CreateSearchCriteriaFromRequest(request));

            return Json(data);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_ESports, Policy = Constant.Policy.Read)]
        public JsonResult SearchAccountPaging(SearchRequest request)
        {
            var data = _betRestrictionService.SearchDataGrid<AccountModel>(CreateSearchCriteriaFromRequest(request));

            return Json(data);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_ESports, Policy = Constant.Policy.Host)]
        public JsonResult AddAccount(AccountRequest request)
        {
            var data = _betRestrictionService.ExecuteNonQuery(new AddAccountCriteria
            {
                UserName = request.UserName,
                LiveBetAccept = request.LiveBetAccept,
                SeizeBetTicket = request.SeizeBetTicket,
                LeagueGroupId = request.LeagueGroupId,
                FollowAccount = request.FollowAccount,
                AccumulateBalance = request.AccumulateBalance,
                SportType = _sportType,
                UserId = CurrentSession.UserId,
                IP = CurrentContext.IP
            });

            return Json(data);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_ESports, Policy = Constant.Policy.Read)]
        public JsonResult GetAccountDetail(int custId)
        {
            var data = _betRestrictionService.GetAccountDetailForEdit(custId, _sportType);
            return Json(data);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_ESports, Policy = Constant.Policy.Host)]
        public JsonResult EditAccount(AccountRequest request)
        {
            var data = _betRestrictionService.ExecuteNonQuery(new EditAccountCriteria
            {
                CustId = request.CustId,
                LiveBetAccept = request.LiveBetAccept,
                SeizeBetTicket = request.SeizeBetTicket,
                FollowAccount = request.FollowAccount,
                LeagueGroupId = request.LeagueGroupId,
                AccumulateBalance = request.AccumulateBalance,
                SportType = _sportType,
                UserId = CurrentSession.UserId,
                IP = CurrentContext.IP
            });

            return Json(data);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.BetRestriction_ESports, Policy = Constant.Policy.Host)]
        public JsonResult RemoveAccount(int custId)
        {
            var data = _betRestrictionService.ExecuteNonQuery(new RemoveAccountCriteria
            {
                CustId = custId,
                SportType = _sportType,
                UserId = CurrentSession.UserId,
                IP = CurrentContext.IP
            });

            return Json(data);
        }

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
