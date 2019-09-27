namespace Fanex.Nap.CMS.Customers.Params.PossessStatistics
{
    public class UpdateMonitorStatusParam : LogParam
    {
        public UpdateMonitorStatusParam(
            string customerList,
            bool isMonitored,
            int userId,
            string ip)
            : base(userId, ip)
        {
            CustomerList = customerList;
            IsMonitored = isMonitored;
        }

        public string CustomerList { get; }

        public bool IsMonitored { get; }
    }
}