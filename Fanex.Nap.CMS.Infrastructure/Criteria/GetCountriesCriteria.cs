namespace Fanex.Nap.CMS.Infrastructure.Criteria
{
    using Fanex.Data.Repository;

    public class GetCountriesCriteria : CriteriaBase
    {
        public override string GetSettingKey()
            => "Member_GetAllCountry";

        public override bool IsValid()
            => true;
    }
}
