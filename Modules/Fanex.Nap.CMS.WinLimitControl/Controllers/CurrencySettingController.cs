namespace Fanex.Nap.CMS.WinLimitControl.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
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

    public class CurrencySettingController : BaseController
    {
        private readonly ISettingService _settingService;
        private readonly ICurrencySettingService _globalSettingService;

        public CurrencySettingController()
            : this(null, null)
        {
        }

        public CurrencySettingController(ISettingService settingService, ICurrencySettingService globalSettingService)
        {
            _settingService = settingService ?? new SettingService();
            _globalSettingService = globalSettingService ?? new CurrencySettingService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.CurrencyWinLimitSetting, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            PopulateCurrencyAndGame();
            ViewBag.HostPermission = HasRight(CMSFunctionKeys.CurrencyWinLimitSetting, Constant.Policy.Host);
            return View("~/Areas/WinLimitControl/Views/CurrencySetting/Index.cshtml");
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.CurrencyWinLimitSetting, Policy = Constant.Policy.Read)]
        public ActionResult Search(GetCurrencyWinLimitCriteria criteria)
        {
            var globalWinLimits = _globalSettingService.GetGlobalWinLimits(criteria);
            return Json(globalWinLimits, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.CurrencyWinLimitSetting, Policy = Constant.Policy.Host)]
        public ActionResult GetExcludedCurrencies(int? currency)
        {
            IList<BaseSetting> currencies = null;
            IList<BaseSetting> games = null;

            int currencyId = currency ?? 0;
            if (currencyId == 0)
            {
                currencies = _globalSettingService.GetExcludedCurrencies();
                if (currencies.Count > 0)
                {
                    currencyId = currencies[0].Id;
                }
            }

            if (currencyId != 0)
            {
                games = _globalSettingService.GetUnsetGames(currencyId);
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
                games = _globalSettingService.GetExcludedGames();
                if (games.Count > 0)
                {
                    gameId = games[0].Id;
                }
            }

            if (gameId != 0)
            {
                currencies = _globalSettingService.GetUnsetCurrencies(gameId);
            }

            var data = new { Currencies = currencies, Games = games };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.CurrencyWinLimitSetting, Policy = Constant.Policy.Host)]
        public ActionResult AddCurrency(BaseSetting currency, IList<AddCurrencyWinLimitCriteria> globalWinLimits)
        {
            var results = _globalSettingService.AddCurrency(currency, globalWinLimits);
            return Json(results, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.CurrencyWinLimitSetting, Policy = Constant.Policy.Host)]
        public ActionResult AddGame(BaseSetting game, IList<AddCurrencyWinLimitCriteria> globalWinLimits)
        {
            var results = _globalSettingService.AddGame(game, globalWinLimits);
            return Json(results, JsonRequestBehavior.DenyGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.CurrencyWinLimitSetting, Policy = Constant.Policy.Host)]
        public ActionResult GetGreaterPlayerWinLimits(GetGreaterPlayerWinLimitCriteria criteria)
        {
            var winlimits = _globalSettingService.GetGreaterPlayerWinLimits(criteria);
            return Json(winlimits, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.CurrencyWinLimitSetting, Policy = Constant.Policy.Host)]
        public ActionResult Update(UpdateCurrencyWinLimitCriteria criteria)
        {
            var result = _globalSettingService.UpdateGlobalWinLimit(criteria);
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
