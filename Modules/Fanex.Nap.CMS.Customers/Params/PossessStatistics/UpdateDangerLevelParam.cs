namespace Fanex.Nap.CMS.Customers.Params.PossessStatistics
{
    using System.Collections.Generic;

    public class UpdateDangerLevelParam : LogParam
    {
        public UpdateDangerLevelParam(
            IDictionary<int, string> selectedCustomers,
            bool updateDownline,
            byte dangerType,
            int dangerLevel,
            int userId,
            string ip)
            : base(userId, ip)
        {
            SelectedCustomers = selectedCustomers;
            DangerType = dangerType;
            UpdateDownline = updateDownline;
            DangerLevel = dangerLevel;
        }

        public IDictionary<int, string> SelectedCustomers { get; }

        public bool UpdateDownline { get; }

        public byte DangerType { get; }

        public int DangerLevel { get; }
    }
}