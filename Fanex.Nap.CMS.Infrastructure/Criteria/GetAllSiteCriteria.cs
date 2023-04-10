namespace Fanex.Nap.CMS.Infrastructure.Criteria
{
    using Fanex.NAP.Framework.Data;

    internal sealed class GetAllSiteCriteria : CriteriaBase
    {
        public GetAllSiteCriteria(int userId)
            => UserId = userId;

        public int UserId { get; }

        public override string GetSettingKey()
            => "Acc_SiteSel";
    }
}
