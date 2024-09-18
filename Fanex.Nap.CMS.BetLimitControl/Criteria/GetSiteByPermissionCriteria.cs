namespace Fanex.NAP.CMS.BetLimitControl.Criteria
{
    using Fanex.Data.Repository;

    public class GetSiteByPermissionCriteria : CriteriaBase
    {
        public GetSiteByPermissionCriteria(string permission)
        {
            Permission = permission;
        }

        public string Permission { get; set; }

        public override string GetSettingKey() => "NAP_BLControl_SiteByPermissionSelect";

        public override bool IsValid() => true;
    }
}
