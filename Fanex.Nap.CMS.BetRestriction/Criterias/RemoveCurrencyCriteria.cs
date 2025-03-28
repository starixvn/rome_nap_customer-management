namespace Fanex.Nap.CMS.BetRestriction.Criterias
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class RemoveCurrencyCriteria : CommandBaseCriteria
    {
        public int ExchangeId { get; set; }

        public int Type { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_BetRestriction_CurrencyDel";
        }

        public override bool IsValid()
            => true;
    }
}
