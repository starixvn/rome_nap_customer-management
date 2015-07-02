namespace Fanex.Nap.CMS.WinLimitControl.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.Settings.Services;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.PlayerWinLimitManagement;
    using Fanex.Nap.CMS.WinLimitControl.Models.PlayerWinLimitManagement;
    using Fanex.Nap.CMS.WinLimitControl.Resources;
    using Fanex.Nap.CMS.WinLimitControl.Services.PlayerWinLimitManagement;

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

        public ActionResult Index()
        {
            PopulateCurrencyAndGame();
            return View("~/Areas/WinLimitControl/Views/PlayerSetting/Index.cshtml");
        }

        public ActionResult Search(GetPlayerWinLimitCriteria criteria)
        {
            var playerWinLimits = _playerSettingService.GetPlayerWinLimits(criteria);
            return Json(playerWinLimits, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchPlayer(GetUnsetGameByPlayerCriteria criteria)
        {
            var unsetGames = _playerSettingService.GetUnsetGameByPlayer(criteria);
            return Json(unsetGames, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Add(AddOrUpdatePlayerWinLimit model)
        {
            var result = new Fanex.Nap.CMS.Infrastructure.Models.Result(0, "Success!!!");
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult Update(AddOrUpdatePlayerWinLimit model)
        {
            var result = new Fanex.Nap.CMS.Infrastructure.Models.Result(0, "Success!!!");
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult Delete(DeletePlayerWinLimitCriteria criteria)
        {
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
