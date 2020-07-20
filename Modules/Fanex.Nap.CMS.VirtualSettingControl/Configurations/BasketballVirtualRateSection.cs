namespace Fanex.Nap.CMS.VirtualSettingControl.Configurations
{
    using System.Configuration;

    class BasketballVirtualRateSection : ConfigurationSection
    {
        private static BasketballVirtualRateSection instance = null;

        public static BasketballVirtualRateSection Instance
        {
            get
            {
                instance = instance ?? ConfigurationManager.GetSection("BasketballVirtualRateSection") as BasketballVirtualRateSection;

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
