namespace Fanex.NAP.CMS.DefaultMemberBetLimit.Criteria
{
    using Fanex.Data.Repository;

    public class InsertCriteria : NonQueryCommand
    {
        public int BetLimitTypeId { get; set; }

        public string Username { get; set; }

        public decimal MaxPerMatch { get; set; }

        public decimal MaxBet { get; set; }

        public string IP { get; set; }

        public int UserId { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMsg { get; set; }

        public override bool IsValid() => BetLimitTypeId > 0 &&
            !string.IsNullOrWhiteSpace(Username) &&
            MaxPerMatch > 0 &&
            MaxBet > 0 &&
            UserId > 0 &&
            !string.IsNullOrWhiteSpace(IP);

        public override string GetSettingKey() => "NAP_BLControl_DefaultSetting_Ins";
    }
}
