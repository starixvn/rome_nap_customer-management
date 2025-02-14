namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.Data.Repository;

    internal class GetCustomerClassCriteria : CriteriaBase
    {
        public override string GetSettingKey()
            => "Nap_LM_CustomerClassSel";

        public override bool IsValid()
            => true;
    }
}
