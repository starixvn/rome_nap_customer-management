using Fanex.Nap.CMS.BetRestriction.Models.Management;
using System.Collections.Generic;

namespace Fanex.Nap.CMS.BetRestriction.ViewModels.Management
{
    public class BetRestrictionSettingViewModel
    {
        public List<BetRestrictionSettingModel> AccountSession { get; set; }

        public List<BetRestrictionSettingModel> SiteSession { get; set; }

        public List<BetRestrictionSettingModel> CurrencySession { get; set; }

        public BetRestrictionSettingViewModel()
        {
            AccountSession = new List<BetRestrictionSettingModel>();
            SiteSession = new List<BetRestrictionSettingModel>();
            CurrencySession = new List<BetRestrictionSettingModel>();
        }
    }
}
