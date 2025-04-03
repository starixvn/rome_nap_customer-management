using Fanex.Nap.CMS.CustomerClass.Requests;

namespace Fanex.Nap.CMS.CustomerClass.Criteria.UtilityOffer
{
    internal class GetUtilityOfferCriteria : QueryCriteriaBase
    {
        public GetUtilityOfferCriteria(SearchUtilityOfferingRequest request)
        {
        }

        public string CustomerClasses { get; set; }
        public string Sites { get; set; }
        public string Currencies { get; set; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassUtilityOfferSel";
    }
}
