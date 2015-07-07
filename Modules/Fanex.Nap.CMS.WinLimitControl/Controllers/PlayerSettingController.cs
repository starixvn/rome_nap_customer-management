namespace Fanex.Nap.CMS.WinLimitControl.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.Settings.Services;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.PlayerSetting;
    using Fanex.Nap.CMS.WinLimitControl.Models.PlayerSetting;
    using Fanex.Nap.CMS.WinLimitControl.Resources;
    using Fanex.Nap.CMS.WinLimitControl.Services.PlayerSetting;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Framework.Data;

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
            var playerWinLimits = _playerSettingService.GetPlayerWinLimits(criteria);
            return Json(
                new { TotalRow = criteria.TotalRow, PlayerWinLimits = playerWinLimits },
                JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.PlayerWinLimitManagement, Policy = Constant.Policy.Host)]
        public ActionResult SearchPlayer(GetUnsetGameByPlayerCriteria criteria)
        {
            var unsetGames = _playerSettingService.GetUnsetGameByPlayer(criteria);
            return Json(unsetGames, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.PlayerWinLimitManagement, Policy = Constant.Policy.Host)]
        public ActionResult GetPlayerWinLimits(GetPlayerWinLimitForUpdateCriteria criteria)
        {
            var winlimits = _playerSettingService.GetPlayerWinLimitsForUpdate(criteria);
            return Json(winlimits, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.PlayerWinLimitManagement, Policy = Constant.Policy.Host)]
        public ActionResult Add(AddOrUpdatePlayerWinLimit model)
        {
            var result = _playerSettingService.AddPlayerWinLimit(ToAddPlayerWinLimitCriteria(model));
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.PlayerWinLimitManagement, Policy = Constant.Policy.Host)]
        public ActionResult Update(AddOrUpdatePlayerWinLimit model)
        {
            var result = _playerSettingService.UpdatePlayerWinLimit(ToUpdatePlayerWinLimitCriteria(model));
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.PlayerWinLimitManagement, Policy = Constant.Policy.Host)]
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

        [NonAction]
        private IList<AddPlayerWinLimitCriteria> ToAddPlayerWinLimitCriteria(AddOrUpdatePlayerWinLimit model)
        {
            IList<AddPlayerWinLimitCriteria> criteria = new List<AddPlayerWinLimitCriteria>();
            foreach (var winlimit in model.WinLimits)
            {
                criteria.Add(new AddPlayerWinLimitCriteria 
                {
                    Player = model.Player.Id,
                    Game = winlimit.Id,
                    PlayerWinLimit = winlimit.PlayerWinLimit
                });    
            }

            return criteria;
        }

        [NonAction]
        private IList<UpdatePlayerWinLimitCriteria> ToUpdatePlayerWinLimitCriteria(AddOrUpdatePlayerWinLimit model)
        {
            IList<UpdatePlayerWinLimitCriteria> criteria = new List<UpdatePlayerWinLimitCriteria>();
            foreach (var winlimit in model.WinLimits)
            {
                criteria.Add(new UpdatePlayerWinLimitCriteria
                {
                    Player = model.Player.Id,
                    Game = winlimit.Id,
                    PlayerWinLimit = winlimit.PlayerWinLimit
                });
            }

            return criteria;
        }
    }
}
