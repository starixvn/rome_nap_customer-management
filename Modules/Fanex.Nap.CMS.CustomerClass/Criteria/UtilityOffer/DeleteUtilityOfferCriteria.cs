namespace Fanex.Nap.CMS.CustomerClass.Criteria.UtilityOffer
{
    internal class DeleteUtilityOfferCriteria : CustomerClassCommandBaseCriteria
    {
        internal DeleteUtilityOfferCriteria(int policyId, int userId, string ip) : base(userId, ip)
        {
            PolicyID = policyId;
        }

        public int PolicyID { get; }

        public override bool IsValid()
            => PolicyID > 0;

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassUtilityOfferDel";
    }
}
