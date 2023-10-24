namespace Fanex.Nap.CMS.CustomerClass.Criteria.UtilityOffer
{
    using Fanex.Nap.CMS.CustomerClass.Requests;
    
    internal class EditUtilityOfferCriteria : AddUtilityOfferCriteria
    {
        internal EditUtilityOfferCriteria(AddEditUtilityOfferRequest request, int userId, string ip) : base(request, userId, ip)
        {
        }

        public override bool IsValid()
            => PolicyID > 0 && base.IsValid();

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassUtilityOfferUpd";
    }
}
