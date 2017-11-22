namespace Fanex.NAP.CMS.Criteria.LossMonitor
{
    using Data.Repository;

    public class GetGroupDownlinesCriteria : CriteriaBase
    {
        public int GroupHeadlineId { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_LimitLoss_MonitorGroupHeadline";
        }

        public override bool IsValid()
        {
            return GroupHeadlineId > 0;
        }
    }
}
