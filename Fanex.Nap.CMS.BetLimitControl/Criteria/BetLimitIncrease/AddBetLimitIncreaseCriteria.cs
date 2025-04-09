namespace Fanex.NAP.CMS.BetLimitControl.Criteria.BetLimitIncrease
{
    using Fanex.Nap.CMS.Infrastructure.Configurations;
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class AddBetLimitIncreaseCriteria : CommandBaseCriteria
    {
        public string CustIDs { get; set; }

        public string SportTypes { get; set; }

        public string Bettypes { get; set; }

        public decimal Multiplier { get; set; }

        public string Note { get; set; }

        public int UserID { get; set; }

        public bool IsTest => AppSettings.IsTest;

        public string IP { get; set; }

        public override string GetSettingKey()
            => "NAP_CM_SVIPMaxBetPlayerInsert";

        public override bool IsValid()
            => true;
    }
}
