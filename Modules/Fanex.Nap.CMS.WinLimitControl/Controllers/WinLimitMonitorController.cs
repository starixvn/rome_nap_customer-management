namespace Fanex.Nap.CMS.WinLimitControl.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.Settings.Models;
    using Fanex.Nap.CMS.Settings.Services;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.WinLimitMonitor;
    using Fanex.Nap.CMS.WinLimitControl.Resources;
    using Fanex.Nap.CMS.WinLimitControl.Services.WinLimitMonitor;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    public class WinLimitMonitorController : BaseController
    {
        private readonly ISettingService _settingService;

        private readonly IWinLimitMonitorService _winLimitMonitorService;

        public WinLimitMonitorController()
            : this(null, null)
        {
        }

        public WinLimitMonitorController(ISettingService settingService, IWinLimitMonitorService winLimitMonitorService)
        {
            _settingService = settingService ?? new SettingService();
            _winLimitMonitorService = winLimitMonitorService ?? new WinLimitMonitorService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.WinLimitMonitor, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            PopulateCurrencyAndGame();
            ViewBag.HostPermission = HasRight(CMSFunctionKeys.WinLimitMonitor, Constant.Policy.Host);
            return View("~/Areas/WinLimitControl/Views/WinLimitMonitor/Index.cshtml");
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.WinLimitMonitor, Policy = Constant.Policy.Read)]
        public ActionResult Search(GetWinLimitMonitorCriteria criteria)
        {
            if (criteria.Username != null)
            {
                criteria.Username = criteria.Username.Trim();
            }

            criteria.PageIndex++;
            object winlimits;
            if (criteria.IsBlocked)
            {
                winlimits = _winLimitMonitorService.GetBlockedWinLimits(criteria);    
            }
            else
            {
                winlimits = _winLimitMonitorService.GetUnblockedWinLimits(criteria);
            }

            return Json(new { TotalRow = criteria.TotalRow, Winlimits = winlimits }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.WinLimitMonitor, Policy = Constant.Policy.Host)]
        public ActionResult BlockUnblockPlayers(BlockUnblockPlayersCriteria criteria)
        {
            var playerWinLimitMonitors = _winLimitMonitorService.BlockUnblockPlayers(criteria);
            return Json(playerWinLimitMonitors, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.WinLimitMonitor, Policy = Constant.Policy.Read)]
        public ActionResult ViewHistory(GetWinLimitHistoryCriteria criteria)
        {
            var histories = _winLimitMonitorService.GetWinLimitHistories(criteria);
            return Json(histories, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private void PopulateCurrencyAndGame()
        {
            var currencies = _settingService.GetCurrencies();
            var games = _settingService.GetGames();
            var statuses = new List<BaseSetting>
            {
                new BaseSetting { Id = 1, Name = WinLimitControlLabels.Blocked },
                new BaseSetting { Id = 2, Name = WinLimitControlLabels.Unblocked }
            };

            var all = new SelectListItem { Value = "0", Text = WinLimitControlLabels.All };
            var selectListCurrencies = currencies.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToList();
            selectListCurrencies.Insert(0, all);

            var selectListGames = games.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToList();
            selectListGames.Insert(0, all);

            ViewBag.Currencies = selectListCurrencies;
            ViewBag.Games = selectListGames;
            ViewBag.Statuses = new SelectList(statuses, "Id", "Name");
        }
    }
}
