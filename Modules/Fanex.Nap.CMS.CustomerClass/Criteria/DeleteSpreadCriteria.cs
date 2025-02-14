namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class DeleteSpreadCriteria : CustomerClassCommandBaseCriteria
    {
        internal DeleteSpreadCriteria(int policyID, int userId, string ip) : base(userId, ip)
        {
            PolicyID = policyID;
        }

        public int PolicyID { get; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassSpreadDel";
    }
}
