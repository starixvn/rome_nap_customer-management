namespace Fanex.Nap.CMS.Customers.Criteria.PossessStatistics
{
    public class UpdateMonitorStatusCriteria : NonQueryBaseCriteria
    {
        public string CustIdList { get; set; }

        public bool IsMonitorPT { get; set; }

        public override string GetSettingKey() => "CS_Customer_MonitorPT_Upd";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(CustIdList);
    }
}