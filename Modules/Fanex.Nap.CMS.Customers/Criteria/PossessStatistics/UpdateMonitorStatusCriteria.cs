namespace Fanex.Nap.CMS.Customers.Criteria.PossessStatistics
{
    public class UpdateMonitorStatusCriteria : NonQueryBaseCriteria
    {
        public UpdateMonitorStatusCriteria(
            string custIdList,
            bool isMonitorPT,
            int userId,
            string ip)
            : base(userId, ip)
        {
            CustIdList = custIdList;
            IsMonitorPT = isMonitorPT;
        }

        public string CustIdList { get; }

        public bool IsMonitorPT { get; }

        public override string GetSettingKey() => "CS_Customer_MonitorPT_Upd";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(CustIdList);
    }
}