namespace Fanex.Nap.CMS.BetRestriction.Criterias.Management
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class EditSettingsCriteria : CommandBaseCriteria
    {
        public int UserId { get; set; }

        public string IP { get; set; }

        public string SettingXML { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_BetRestriction_SettingUpd";
        }

        public override bool IsValid()
        {
            return UserId > 0
                && !string.IsNullOrWhiteSpace(SettingXML)
                && !SettingXML.Equals("<R></R>");
        }
    }
}
