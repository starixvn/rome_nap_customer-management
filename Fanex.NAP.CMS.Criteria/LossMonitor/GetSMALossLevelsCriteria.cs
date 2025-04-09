namespace Fanex.NAP.CMS.Criteria.LossMonitor
{
    using Data.Repository;

    public class GetSMALossLevelsCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "NAP_LimitLoss_ActionConfigSel";
        }

        public override bool IsValid()
            => true;
    }
}