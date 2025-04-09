namespace Fanex.NAP.CMS.BetLimitControl.Criteria.BetLimitIncrease
{
    using Fanex.Nap.CMS.Infrastructure.Configurations;
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class CheckBetLimitIncreaseCriteria : CommandBaseCriteria
    {
        public string Username { get; set; }
        public string SportTypes { get; set; }
        public string Bettypes { get; set; }
        public decimal Multiplier { get; set; }
        public bool IsTest => AppSettings.IsTest;

        public override string GetSettingKey()
            => "NAP_CM_SVIPMaxBetPlayerCheck";

        public override bool IsValid()
            => true;
    }
}
