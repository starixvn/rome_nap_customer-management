namespace Fanex.Nap.CMS.VirtualSettingControl.Configurations
{
    using System.Configuration;

    public class ESportVirtualRateBetTypeCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
            => new ESportVirtualRateBetTypeElement();

        protected override object GetElementKey(ConfigurationElement element)
            => ((ESportVirtualRateBetTypeElement)element).Key;
    }
}
