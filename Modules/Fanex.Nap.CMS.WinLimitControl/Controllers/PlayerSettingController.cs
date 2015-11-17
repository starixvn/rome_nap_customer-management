namespace Fanex.Nap.CMS.WinLimitControl.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Settings.Services;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.PlayerSetting;
    using Fanex.Nap.CMS.WinLimitControl.Resources;
    using Fanex.Nap.CMS.WinLimitControl.Services.PlayerSetting;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    public class PlayerSettingController : BaseController
    {
        private readonly ISettingService _settingService;
        private readonly IPlayerSettingService _playerSettingService;

        public PlayerSettingController()
            : this(null, null)
        {
        }

        public PlayerSettingController(ISettingService settingService, IPlayerSettingService playerSettingService)
        {
            _settingService = settingService ?? new SettingService();
            _playerSettingService = playerSettingService ?? new PlayerSettingService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.PlayerWinLimitManagement, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            PopulateCurrencyAndGame();
            ViewBag.HostPermission = HasRight(CMSFunctionKeys.PlayerWinLimitManagement, Constant.Policy.Host);
            return View("~/Areas/WinLimitControl/Views/PlayerSetting/Index.cshtml");
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.PlayerWinLimitManagement, Policy = Constant.Policy.Read)]
        public ActionResult Search(GetPlayerWinLimitCriteria criteria)
        {
            if (criteria.Username != null)
            {
                criteria.Username = criteria.Username.Trim();
            }

            criteria.PageIndex++;
            var playerWinLimits = _playerSettingService.GetPlayerSettings(criteria);
            return Json(
                new { TotalRow = criteria.TotalRow, PlayerWinLimits = playerWinLimits },
                JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.PlayerWinLimitManagement, Policy = Constant.Policy.Host)]
        public ActionResult SearchPlayer(GetUndefinedGameByPlayerCriteria criteria)
        {
            var undefinedSettings = _playerSettingService.GetUndefinedGameByPlayer(criteria);
            if (criteria.ErrCode == 0 && undefinedSettings.Count > 0)
            {
                return Json(undefinedSettings, JsonRequestBehavior.AllowGet);
            }

            var result = new Result(2, WinLimitControlLabels.NoMoreGameToSetPlayerWinLimit);
            if (criteria.ErrCode != 0)
            {
                result.Code = criteria.ErrCode;
                if (criteria.ErrCode == 3)
                {
                    result.Message = WinLimitControlLabels.InvalidUsername;
                }
                else if (criteria.ErrCode == 4)
                {
                    result.Message = string.Format(WinLimitControlLabels.MessageGoToCurrencySettingPage, criteria.ErrMsg, "CurrencySetting");  
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.PlayerWinLimitManagement, Policy = Constant.Policy.Read)]
        public ActionResult AutoSuggestion(string term)
        {
            var criteria = new CustomerSuggestionCriteria(term);
            var customers = _playerSettingService.CustomerAutoSuggestion(criteria);
            return Json(customers, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.PlayerWinLimitManagement, Policy = Constant.Policy.Host)]
        public ActionResult Add(AddPlayerWinLimitCriteria criteria)
        {
            var result = _playerSettingService.AddPlayerWinLimit(criteria);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.PlayerWinLimitManagement, Policy = Constant.Policy.Host)]
        public ActionResult Update(UpdatePlayerWinLimitCriteria criteria)
        {
            criteria.UserId = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;

            var result = _playerSettingService.UpdatePlayerWinLimit(criteria);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.PlayerWinLimitManagement, Policy = Constant.Policy.Host)]
        public ActionResult Delete(DeletePlayerWinLimitCriteria criteria)
        {
            criteria.UserId = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;

            var result = _playerSettingService.DeletePlayerWinLimit(criteria);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [NonAction]
        private void PopulateCurrencyAndGame()
        {
            var currencies = _settingService.GetCurrencies();
            var games = _settingService.GetGames();

            var all = new SelectListItem { Value = "0", Text = WinLimitControlLabels.All };
            var selectListCurrencies = currencies.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToList();
            selectListCurrencies.Insert(0, all);

            var selectListGames = games.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToList();
            selectListGames.Insert(0, all);

            ViewBag.Currencies = selectListCurrencies;
            ViewBag.Games = selectListGames;
        }
    }
}
