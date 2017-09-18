namespace Fanex.NAP.CMS.Criteria.LossMonitor
{
    using Data.Repository;

    public class InsertSMACriteria : CriteriaBase
    {
        public string Username { get; set; }

        public decimal MaxLoss { get; set; }

        [SpParam(SqlParamName = "Mode")]
        public bool IsGroupHeadline { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_LimitLoss_SMAIns";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Username) &&
                MaxLoss <= 0 &&
                UserId > 0 &&
                !string.IsNullOrWhiteSpace(IP);
        }
    }
}
