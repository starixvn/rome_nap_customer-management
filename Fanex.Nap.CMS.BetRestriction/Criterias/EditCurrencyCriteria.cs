namespace Fanex.Nap.CMS.BetRestriction.Criterias
{
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class EditCurrencyCriteria : CommandBaseCriteria
    {
        public int ExchangeId { get; set; }

        public int UserId { get; set; }

        public int CurrentType { get; set; }

        public int NewType { get; set; }

        public short? LiveBetAccept { get; set; }

        public int? SeizeBetTicket { get; set; }

        public string FollowAccount { get; set; }

        public byte AccumulateBalance { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_BetRestriction_CurrencyUpd";
        }

        public override bool IsValid()
            => true;
    }
}
