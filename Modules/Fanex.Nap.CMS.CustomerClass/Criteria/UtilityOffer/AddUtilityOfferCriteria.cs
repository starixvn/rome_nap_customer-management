namespace Fanex.Nap.CMS.CustomerClass.Criteria.UtilityOffer
{
    using Fanex.Nap.CMS.CustomerClass.Requests;

    internal class AddUtilityOfferCriteria : CommandCriteriaBase
    {
        internal AddUtilityOfferCriteria(AddEditUtilityOfferRequest request, int userId, string ip) : base(userId, ip)
        {
            PolicyID = request.PolicyId;
            PolicyTitle = request.PolicyTitle;
            CustomerClasses = request.CustomerClass;
            Currencies = request.Currencies;
            Sites = request.Sites;
            UtilityOffering = request.UtilityOffering;
        }

        public int PolicyID { get; set; }
        public string PolicyTitle { get; }
        public string CustomerClasses { get; }
        public string Currencies { get; }
        public string Sites { get; }
        public string UtilityOffering { get; }

        public override bool IsValid()
            => !(string.IsNullOrWhiteSpace(PolicyTitle)
            || string.IsNullOrWhiteSpace(CustomerClasses)
            || string.IsNullOrWhiteSpace(Currencies)
            || string.IsNullOrWhiteSpace(Sites)
            || string.IsNullOrWhiteSpace(UtilityOffering));

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassUtilityOfferIns";
    }
}
