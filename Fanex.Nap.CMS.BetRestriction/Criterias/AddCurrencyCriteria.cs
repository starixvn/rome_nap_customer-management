namespace Fanex.Nap.CMS.BetRestriction.Criterias
{
    using Fanex.Data.Repository;

    public class AddCurrencyCriteria : CriteriaBase
    {
        public int ExchangeId { get; set; }

        public int UserId { get; set; }

        public int Type { get; set; }

        public int LiveBetAccept { get; set; }

        public int? SeizeBetTicket { get; set; }

        public string FollowAccount { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_BetRestriction_CurrencyIns";
        }
    }
}
