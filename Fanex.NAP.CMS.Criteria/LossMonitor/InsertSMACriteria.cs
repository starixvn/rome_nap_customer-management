namespace Fanex.NAP.CMS.Criteria.LossMonitor
{
    using Fanex.Data;
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class InsertSMACriteria : CommandBaseCriteria
    {
        public string Username { get; set; }

        public decimal MaxLoss { get; set; }

        public bool NotifyLetsTalk { get; set; }

        [SpParam(SqlParamName = "Mode")]
        public bool IsGroupHeadline { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

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
