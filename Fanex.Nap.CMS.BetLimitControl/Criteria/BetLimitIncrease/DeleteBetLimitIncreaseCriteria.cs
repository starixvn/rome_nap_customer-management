namespace Fanex.NAP.CMS.BetLimitControl.Criteria.BetLimitIncrease
{
    using Fanex.Nap.CMS.Infrastructure.Configurations;
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class DeleteBetLimitIncreaseCriteria : CommandBaseCriteria
    {
        public string CustIDs { get; set; }

        public string SportTypes { get; set; }

        public string Bettypes { get; set; }

        public int UserID { get; set; }

        public bool IsTest => AppSettings.IsTest;

        public string IP { get; set; }

        public override string GetSettingKey()
            => "NAP_CM_SVIPMaxBetPlayerDel";

        public override bool IsValid()
            => true;
    }
}
