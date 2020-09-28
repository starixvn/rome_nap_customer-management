namespace Fanex.NAP.CMS.BetLimitControl.Criteria
{
    using Fanex.Data.Repository;

    public class AddBetLimitControlCriteria : CriteriaBase
    {
        public string Usernames { get; set; }

        public int BetLimitTypeID { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
            => "NAP_BLControl_CustInsert";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Usernames) && BetLimitTypeID > 0 && MaxBet <= MaxPerMatch && UserId > 0 && !string.IsNullOrWhiteSpace(IP);
    }
}