using Fanex.Nap.CMS.Infrastructure.Resources;
using Fanex.Nap.CMS.Infrastructure.Web;

namespace Fanex.Nap.CMS.BetRestriction.Models.Management
{
    public class BetRestrictionSettingModel
    {
        public int SettingId { get; set; }

        public int SportType { get; set; }

        public string SportName => CacheRef.GetSportName(SportType);

        public string RestrictionLevel { get; set; }

        public bool IsLicensee { get; set; }

        public string Type => IsLicensee ? Labels.Licensees : Labels.Credits;

        public bool LiveBetAcceptStatus { get; set; }

        public bool SeizeBetTicketStatus { get; set; }

        public bool BetLimitStatus { get; set; }
    }
}
