using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.Infrastructure.Resources;

namespace Fanex.Nap.CMS.BetRestriction.Models
{
    public class BetRestrictionCurrencyModel :CurrencyModel
    {
        public int Type { get; set; }

        public string TypeName 
        {
            get
            {
                switch (Type)
                {
                    case 0:
                        return Labels.All;
                    case 1:
                        return Labels.Credits;
                    case 2:
                        return Labels.Licensees;
                    default:
                        return string.Empty;
                }
            }
        }

        public short? LiveBetAccept { get; set; }

        public byte? SeizeBetTicket { get; set; }

        public string FollowAccount { get; set; }

        public byte AccumulateBalance { get; set; }
    }
}
