namespace Fanex.NAP.CMS.BetLimitControl.Criteria.BetLimitIncrease
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Configurations;

    public class AddBetLimitIncreaseCriteria : CriteriaBase
    {
        public string CustIDs { get; set; }

        public string SportTypes { get; set; }

        public string Bettypes { get; set; }

        public decimal Multiplier { get; set; }

        public string Note { get; set; }

        public int UserID { get; set; }

        public bool IsTest => AppSettings.IsTest;

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "NAP_CM_SVIPMaxBetPlayerInsert";

        public override bool IsValid()
            => true;
    }
}
