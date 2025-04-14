namespace Fanex.Nap.CMS.CustomerClass.Criteria.PeakHour
{
    using Fanex.Nap.CMS.Infrastructure.Configurations;
    using Fanex.NAP.Framework.Data;

    internal class GetPeakHourCriteria : CriteriaBase
    {
        public bool IsTest { get; } = AppSettings.IsTest;

        public override string GetSettingKey()
            => "NAP_CM_CCPeakHourRangerSel";
    }
}
