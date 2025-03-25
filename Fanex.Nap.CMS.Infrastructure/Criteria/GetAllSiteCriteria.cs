namespace Fanex.Nap.CMS.Infrastructure.Criteria
{
    using Fanex.Data.Repository;

    internal sealed class GetAllSiteCriteria : CriteriaBase
    {
        public GetAllSiteCriteria(int? userId)
            => UserId = userId;

        public int? UserId { get; }

        public override string GetSettingKey()
            => "Acc_SiteSel";

        public override bool IsValid()
            => true;
    }
}
