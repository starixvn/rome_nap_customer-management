namespace Fanex.Nap.CMS.BetRestriction.Criterias
{
    using Fanex.Data.Repository;

    public class RemoveCurrencyCriteria : CriteriaBase
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
