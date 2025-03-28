namespace Fanex.Nap.CMS.BetRestriction.Criterias
{
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class RemoveSiteCriteria : CommandBaseCriteria
    {
        public int SiteId { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_BetRestriction_SiteDel";
        }

        public override bool IsValid()
            => true;
    }
}
