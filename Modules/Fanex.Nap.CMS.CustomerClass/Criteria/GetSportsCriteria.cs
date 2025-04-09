namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.Data.Repository;

    internal class GetSportsCriteria : CriteriaBase
    {
        public override string GetSettingKey()
            => "Nap_LM_SportTypeSel";

        public override bool IsValid()
            => true;
    }
}
