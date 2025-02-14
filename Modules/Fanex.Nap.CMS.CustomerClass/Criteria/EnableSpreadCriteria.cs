namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class EnableSpreadCriteria : CustomerClassCommandBaseCriteria
    {
        internal EnableSpreadCriteria(int policyID, bool enable, int userId, string ip) : base(userId, ip)
        {
            PolicyID = policyID;
            Enable = enable;
        }

        public int PolicyID { get; }
        public bool Enable { get; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassSpreadEnable";
    }
}
