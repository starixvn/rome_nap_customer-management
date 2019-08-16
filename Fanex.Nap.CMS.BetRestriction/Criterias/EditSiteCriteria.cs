namespace Fanex.Nap.CMS.BetRestriction.Criterias
{
    using Fanex.Data.Repository;

    public class EditSiteCriteria : AddSiteCriteria
    {
        public override string GetSettingKey()
        {
            return "NAP_BetRestriction_SiteUpd";
        }
    }
}
