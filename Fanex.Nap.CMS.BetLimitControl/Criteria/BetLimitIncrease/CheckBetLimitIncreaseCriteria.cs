namespace Fanex.NAP.CMS.BetLimitControl.Criteria.BetLimitIncrease
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Configurations;

    public class CheckBetLimitIncreaseCriteria : CriteriaBase
    {
        public string Username { get; set; }
        public string SportTypes { get; set; }
        public string Bettypes { get; set; }
        public decimal Multiplier { get; set; }
        public bool IsTest => AppSettings.IsTest;
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "NAP_CM_SVIPMaxBetPlayerCheck";

        public override bool IsValid()
            => true;
    }
}
