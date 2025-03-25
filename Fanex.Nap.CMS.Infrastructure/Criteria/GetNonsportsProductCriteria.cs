namespace Fanex.Nap.CMS.Infrastructure.Criteria
{
    using Fanex.Data.Repository;

    public class GetNonsportsProductCriteria : CriteriaBase
    {
        public bool IsCredit { get; set; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassNonSportsProduct";

        public override bool IsValid()
            => true;
    }
}
