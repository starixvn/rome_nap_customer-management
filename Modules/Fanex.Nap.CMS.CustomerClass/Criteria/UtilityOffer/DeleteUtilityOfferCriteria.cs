namespace Fanex.Nap.CMS.CustomerClass.Criteria.UtilityOffer
{
    internal class DeleteUtilityOfferCriteria : CustomerClassCommandBaseCriteria
    {
        public int PolicyID { get; set; }

        public override bool IsValid()
            => PolicyID > 0;

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassUtilityOfferDel";
    }
}
