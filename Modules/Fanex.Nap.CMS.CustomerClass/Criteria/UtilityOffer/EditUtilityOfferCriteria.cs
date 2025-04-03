namespace Fanex.Nap.CMS.CustomerClass.Criteria.UtilityOffer
{
    internal class EditUtilityOfferCriteria : AddUtilityOfferCriteria
    {
        public override bool IsValid()
            => PolicyID > 0 && base.IsValid();

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassUtilityOfferUpd";
    }
}
