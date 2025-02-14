namespace Fanex.NAP.CMS.Criteria.LossMonitor
{
    using Data.Repository;
    using Fanex.Data;

    public class UpdateSMAMaxLossCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public decimal MaxLoss { get; set; }

        [SpParam(SqlParamName = "Mode")]
        public bool IsGroupHeadline { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_LimitLoss_SMAMaxLossUpd";
        }

        public override bool IsValid()
        {
            return CustId > 0 &&
                MaxLoss <= 0 &&
                UserId > 0 &&
                !string.IsNullOrWhiteSpace(IP);
        }
    }
}
