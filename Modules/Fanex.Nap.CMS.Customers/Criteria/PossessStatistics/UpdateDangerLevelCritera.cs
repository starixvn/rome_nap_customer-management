namespace Fanex.Nap.CMS.Customers.Criteria.PossessStatistics
{
    public class UpdateDangerLevelCritera : NonQueryBaseCriteria
    {
        public UpdateDangerLevelCritera(
            int custId,
            bool updateDownline,
            byte dangerType,
            int dangerLevel,
            int userId,
            string ip)
            : base(userId, ip)
        {
            CustId = custId;
            UpdateDownline = updateDownline;
            DangerType = dangerType;
            DangerLevel = dangerLevel;
        }

        public int CustId { get; }

        public bool UpdateDownline { get; }

        public byte DangerType { get; }

        public int DangerLevel { get; }

        public override string GetSettingKey() => "Acc_CustomerUpdDangercust";

        public override bool IsValid() => CustId > 0;
    }
}