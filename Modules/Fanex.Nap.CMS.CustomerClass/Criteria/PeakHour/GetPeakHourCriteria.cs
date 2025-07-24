namespace Fanex.Nap.CMS.CustomerClass.Criteria.PeakHour
{
    using Fanex.Nap.CMS.Infrastructure.Configurations;
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    internal class GetPeakHourCriteria : CommandBaseCriteria
    {
        public bool IsTest { get; } = AppSettings.IsTest;

        public override string GetSettingKey()
            => "NAP_CM_CCPeakHourRangerSel";
    }
}
