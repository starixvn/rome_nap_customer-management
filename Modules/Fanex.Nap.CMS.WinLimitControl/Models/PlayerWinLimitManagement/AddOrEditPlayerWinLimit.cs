namespace Fanex.Nap.CMS.WinLimitControl.Models.PlayerWinLimitManagement
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Settings.Models;

    public class AddOrUpdatePlayerWinLimit
    {
        public BaseSetting Player { get; set; }

        public string Currency { get; set; }

        public Result DbResult { get; set; }

        public IList<GameWinLimit> WinLimits { get; set; }
    }
}
