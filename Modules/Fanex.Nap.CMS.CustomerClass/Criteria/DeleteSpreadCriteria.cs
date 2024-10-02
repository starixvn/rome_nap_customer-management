namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class DeleteSpreadCriteria : CommandCriteriaBase
    {
        internal DeleteSpreadCriteria(int policyID, int userId, string ip, byte type) : base(userId, ip)
        {
            PolicyID = policyID;
            Type = type;
        }

        public int PolicyID { get; }
        public byte Type { get; }

        public override string GetSettingKey()
            => "NAP_CM_CustomerClassSpreadDel";
    }
}
