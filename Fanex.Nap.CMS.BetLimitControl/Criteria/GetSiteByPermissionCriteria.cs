namespace Fanex.NAP.CMS.BetLimitControl.Criteria
{
    using Fanex.Data.Repository;

    public class GetSiteByPermissionCriteria : CriteriaBase
    {
        public GetSiteByPermissionCriteria(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; private set; }

        public override string GetSettingKey() => "NAP_BLControl_SiteByPermissionSelect";

        public override bool IsValid() => UserId > 0;
    }
}
