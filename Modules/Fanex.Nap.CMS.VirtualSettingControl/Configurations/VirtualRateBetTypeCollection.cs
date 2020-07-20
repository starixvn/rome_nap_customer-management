namespace Fanex.Nap.CMS.VirtualSettingControl.Configurations
{
    using System.Configuration;

    public class VirtualRateBetTypeCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
            => new VirtualRateBetTypeElement();

        protected override object GetElementKey(ConfigurationElement element)
            => ((VirtualRateBetTypeElement)element).Key;
    }
}
