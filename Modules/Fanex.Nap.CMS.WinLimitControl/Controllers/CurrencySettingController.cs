namespace Fanex.Nap.CMS.WinLimitControl.Controllers
{
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
            return Json(currencySettings, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.CurrencyWinLimitSetting, Policy = Constant.Policy.Host)]
        public ActionResult Add(AddCurrencyWinLimitCriteria criteria)
        {
            criteria.UserId = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;

            var result = _currencySettingService.AddCurrencySetting(criteria);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.CurrencyWinLimitSetting, Policy = Constant.Policy.Host)]
        public ActionResult Update(UpdateCurrencyWinLimitCriteria criteria)
        {
            criteria.UserId = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;

            var result = _currencySettingService.UpdateCurrencySetting(criteria);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.CurrencyWinLimitSetting, Policy = Constant.Policy.Host)]
        public ActionResult GetGreaterPlayerWinLimits(GetGreaterPlayerWinLimitCriteria criteria)
        {
            var winlimits = _currencySettingService.GetGreaterPlayerWinLimits(criteria);
            return Json(winlimits, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private void PopulateGameAndCurrency()
        {
            var games = _settingService.GetGames();
            var currencies = _settingService.GetCurrencies().Select(c => new BaseSetting { Id = c.Id, Name = c.Name }).ToList();
            currencies.Insert(0, new BaseSetting { Id = 0, Name = WinLimitControlLabels.All });
            
            ViewBag.Games = new SelectList(games, "Id", "Name");
            ViewBag.Currencies = new SelectList(currencies, "Id", "Name");
        }
    }
}
