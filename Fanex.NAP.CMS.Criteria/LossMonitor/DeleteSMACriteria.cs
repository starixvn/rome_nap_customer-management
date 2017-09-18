namespace Fanex.NAP.CMS.Criteria.LossMonitor
{
    using Data.Repository;

    public class DeleteSMACriteria : CriteriaBase
    {
        public int CustId { get; set; }

        [SpParam(SqlParamName = "Mode")]
        public bool IsGroupHeadline { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_LimitLoss_SMADel";
        }

        public override bool IsValid()
        {
            return CustId > 0 &&
                UserId > 0 &&
                !string.IsNullOrWhiteSpace(IP);
        }
    }
}
