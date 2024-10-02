namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class EnableSpreadCriteria : CommandCriteriaBase
    {
        internal EnableSpreadCriteria(int policyID, bool enable, int userId, string ip, byte type) : base(userId, ip)
        {
            PolicyID = policyID;
            Enable = enable;
            Type = type;
        }

        public int PolicyID { get; }
        public bool Enable { get; }
        public byte Type { get; }

        public override string GetSettingKey()
            => "NAP_CM_CustomerClassSpreadEnable";
    }
}
