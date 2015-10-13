namespace Fanex.Nap.CMS.WinLimitControl.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.Settings.Models;
    using Fanex.Nap.CMS.Settings.Services;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.CurrencySetting;
    using Fanex.Nap.CMS.WinLimitControl.Resources;
    using Fanex.Nap.CMS.WinLimitControl.Services.CurrencySetting;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using System.Linq;

    public class CurrencySettingController : BaseController
    {
        private readonly ISettingService _settingService;
        private readonly ICurrencySettingService _currencySettingService;

        public CurrencySettingController()
            : this(null, null)
        {
        }

        public CurrencySettingController(ISettingService settingService, ICurrencySettingService currencySettingService)
        {
            _settingService = settingService ?? new SettingService();
            _currencySettingService = currencySettingService ?? new CurrencySettingService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.CurrencyWinLimitSetting, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            PopulateGameAndCurrency();
            ViewBag.HostPermission = HasRight(CMSFunctionKeys.CurrencyWinLimitSetting, Constant.Policy.Host);
            return View("~/Areas/WinLimitControl/Views/CurrencySetting/Index.cshtml");
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.CurrencyWinLimitSetting, Policy = Constant.Policy.Read)]
        public ActionResult Search(GetCurrencyWinLimitCriteria criteria)
        {
            var currencySettings = _currencySettingService.GetCurrencySettings(criteria);
            return PartialView("~/Areas/WinLimitControl/Views/CurrencySetting/_Search.cshtml", currencySettings);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.CurrencyWinLimitSetting, Policy = Constant.Policy.Host)]
        public ActionResult GetExcludedCurrencies(int? currency)
        {
            IList<BaseSetting> currencies = null;
            IList<BaseSetting> games = null;

            int currencyId = currency ?? 0;
            if (currencyId == 0)
            {
                currencies = _currencySettingService.GetExcludedCurrencies();
                if (currencies.Count > 0)
                {
                    currencyId = currencies[0].Id;
                }
            }

            if (currencyId != 0)
            {
                games = _currencySettingService.GetUnsetGames(currencyId);
            }

            var data = new { Currencies = currencies, Games = games };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.CurrencyWinLimitSetting, Policy = Constant.Policy.Host)]
        public ActionResult GetExcludedGames(int? game)
        {
            IList<BaseSetting> currencies = null;
            IList<BaseSetting> games = null;

            int gameId = game ?? 0;
            if (gameId == 0)
            {
                games = _currencySettingService.GetExcludedGames();
                if (games.Count > 0)
                {
                    gameId = games[0].Id;
                }
            }

            if (gameId != 0)
            {
                currencies = _currencySettingService.GetUnsetCurrencies(gameId);
            }

            var data = new { Currencies = currencies, Games = games };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.CurrencyWinLimitSetting, Policy = Constant.Policy.Host)]
        public ActionResult AddCurrency(BaseSetting currency, IList<AddCurrencyWinLimitCriteria> globalWinLimits)
        {
            AddUserIdAndIP(globalWinLimits);
            var results = _currencySettingService.AddCurrency(currency, globalWinLimits);
            return Json(results, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.CurrencyWinLimitSetting, Policy = Constant.Policy.Host)]
        public ActionResult AddGame(BaseSetting game, IList<AddCurrencyWinLimitCriteria> globalWinLimits)
        {
            AddUserIdAndIP(globalWinLimits);
            var results = _currencySettingService.AddGame(game, globalWinLimits);
            return Json(results, JsonRequestBehavior.DenyGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.CurrencyWinLimitSetting, Policy = Constant.Policy.Host)]
        public ActionResult GetGreaterPlayerWinLimits(GetGreaterPlayerWinLimitCriteria criteria)
        {
            var winlimits = _currencySettingService.GetGreaterPlayerWinLimits(criteria);
            return Json(winlimits, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.CurrencyWinLimitSetting, Policy = Constant.Policy.Host)]
        public ActionResult Update(UpdateCurrencyWinLimitCriteria criteria)
        {
            criteria.UserId = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;
 
            var result = _currencySettingService.UpdateGlobalWinLimit(criteria);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [NonAction]
        private void PopulateGameAndCurrency()
        {
            var games = _settingService.GetGames();
            var currencies = _settingService.GetCurrencies().Select(c => new BaseSetting { Id = c.Id, Name = c.Name }).ToList();
            currencies.Insert(0, new BaseSetting { Id = 0, Name = WinLimitControlLabels.All });
            
            ViewBag.Games = new SelectList(games, "Id", "Name");
            ViewBag.Currencies = new SelectList(currencies, "Id", "Name"); ;
        }

        [NonAction]
        private void AddUserIdAndIP(IList<AddCurrencyWinLimitCriteria> globalWinLimits)
        {
            int userId = CurrentSession.UserId;
            string ip = CurrentContext.IP;

            foreach (var criteria in globalWinLimits)
            {
                criteria.UserId = userId;
                criteria.IP = ip; 
            }
        }
    }
}
