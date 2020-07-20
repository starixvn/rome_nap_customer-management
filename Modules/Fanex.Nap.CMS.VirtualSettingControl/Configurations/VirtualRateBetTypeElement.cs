namespace Fanex.Nap.CMS.VirtualSettingControl.Configurations
{
    using System.Configuration;

    public class VirtualRateBetTypeElement : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        [ConfigurationProperty("leagueId", IsRequired = true)]
        public int LeagueId
        {
            get { return (int)this["leagueId"]; }
            set { this["leagueId"] = value; }
        }

        [ConfigurationProperty("betTypeGroupId", IsRequired = true)]
        public int BetTypeGroupId
        {
            get { return (int)this["betTypeGroupId"]; }
            set { this["betTypeGroupId"] = value; }
        }

        [ConfigurationProperty("betTypeId", IsRequired = true)]
        public int BetTypeId
        {
            get { return (int)this["betTypeId"]; }
            set { this["betTypeId"] = value; }
        }

        [ConfigurationProperty("live", IsRequired = true)]
        public bool Live
        {
            get { return (bool)this["live"]; }
            set { this["live"] = value; }
        }

        [ConfigurationProperty("cell", IsRequired = true)]
        public string Cell
        {
            get { return (string)this["cell"]; }
            set { this["cell"] = value; }
        }
    }
}
