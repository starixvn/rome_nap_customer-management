using Fanex.Nap.CMS.CustomerClass.Requests;

namespace Fanex.Nap.CMS.CustomerClass.Criteria.UtilityOffer
{
    internal class GetUtilityOfferCriteria : QueryCriteriaBase
    {
        public GetUtilityOfferCriteria(SearchUtilityOfferingRequest request)
        {
            CustomerClasses = request.CustomerClass;
            Sites = request.Site;
            Currencies = request.Currency;
        }

        public string CustomerClasses { get; }
        public string Sites { get; }
        public string Currencies { get; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassUtilityOfferSel";
    }
}
