namespace Fanex.Nap.CMS.CustomerClass.Criteria.UtilityOffer
{
    internal class AddUtilityOfferCriteria : CustomerClassCommandBaseCriteria
    {
        public int PolicyID { get; set; }
        public string PolicyTitle { get; set; }
        public string CustomerClasses { get; set; }
        public string Currencies { get; set; }
        public string Sites { get; set; }
        public string UtilityOffering { get; set; }

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
