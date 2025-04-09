namespace Fanex.Nap.CMS.Customers.Criteria.PossessStatistics
{
    public class UpdateDangerLevelCritera : NonQueryBaseCriteria
    {
        public int CustId { get; set; }

        public bool UpdateDownline { get; set; }

        public byte DangerType { get; set; }

        public int DangerLevel { get; set; }

        public static string System => "NAP-CM";

        public override string GetSettingKey() => "Acc_CustomerUpdDangercust";

        public override bool IsValid() => CustId > 0;
    }
}