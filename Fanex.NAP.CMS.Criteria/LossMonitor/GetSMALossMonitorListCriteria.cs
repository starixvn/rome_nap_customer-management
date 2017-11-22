namespace Fanex.NAP.CMS.Criteria.LossMonitor
{
    using Data.Repository;

    public class GetSMALossMonitorListCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "NAP_LimitLoss_MonitorList";
        }
    }
}