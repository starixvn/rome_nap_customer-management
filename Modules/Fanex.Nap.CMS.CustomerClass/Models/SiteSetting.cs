namespace Fanex.Nap.CMS.CustomerClass.Models
{
    internal class SiteSetting
    {
        public SiteSetting(
            string policyTitle,
            string site,
            string currencies,
            string platforms)
        {
            PolicyTitle = policyTitle;
            Site = site;
            Currencies = currencies;
            Platforms = platforms;
        }

        public string PolicyTitle { get; }

        public string Site { get; }

        public string Currencies { get; }

        public bool IsAllCurrencies { get; set; }

        public string Platforms { get; }

        public string PlatformNames { get; set; }
    }
}
