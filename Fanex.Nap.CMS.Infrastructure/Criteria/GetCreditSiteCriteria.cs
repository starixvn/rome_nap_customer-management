namespace Fanex.Nap.CMS.Infrastructure.Criteria
{
    using Fanex.Data.Repository;

    internal sealed class GetCreditSiteCriteria : CriteriaBase
    {
        public override string GetSettingKey()
            => "Acc_SiteSel_Credit";

        public override bool IsValid()
            => true;
    }
}
