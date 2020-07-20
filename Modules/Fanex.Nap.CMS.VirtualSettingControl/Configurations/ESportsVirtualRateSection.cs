namespace Fanex.Nap.CMS.VirtualSettingControl.Configurations
{
    using System.Configuration;

    public class ESportsVirtualRateSection : ConfigurationSection
    {
        private static ESportsVirtualRateSection instance = null;

        public static ESportsVirtualRateSection Instance
        {
            get
            {
                instance = instance ?? ConfigurationManager.GetSection("ESportsVirtualRateSection") as ESportsVirtualRateSection;

                return instance;
            }
        }

        [ConfigurationProperty("BetTypes", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(VirtualRateBetTypeCollection))]
        public VirtualRateBetTypeCollection BetTypes
        {
            get
            {
                return (VirtualRateBetTypeCollection)base["BetTypes"];
            }
        }
    }
}
