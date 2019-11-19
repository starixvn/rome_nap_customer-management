namespace Fanex.NAP.CMS.Criteria.LossMonitor
{
    using System.Collections;
    using Data.Repository;

    public class UpdateLetsTalkNotificationCriteria : CriteriaBase
    {
        private const int UpdateAllMode = 2;
        private const int UpdateHeadlineMode = 1;
        private const int UpdateSMAMode = 0;

        public int CustId { get; set; }

        public bool IsEnable { get; set; }

        [SpParam(SqlParamName = "Mode")]
        public bool IsGroupHeadline { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_LimitLoss_SMANotificationUpd";
        }

        public override bool IsValid()
        {
            return CustId >= 0;
        }

        public override Hashtable GetSpParams()
        {
            var mode = GetUpdateMode();

            var result = new Hashtable
            {
                ["custid"] = CustId,
                ["notifyletstalk"] = IsEnable,
                ["mode"] = mode,
                ["userid"] = UserId,
                ["ip"] = IP
            };

            return result;
        }

        private int GetUpdateMode() => CustId == 0 ? UpdateAllMode :
                            GetHeadlineMode();

        private int GetHeadlineMode() => IsGroupHeadline ? UpdateHeadlineMode :
                                        UpdateSMAMode;
    }
}
