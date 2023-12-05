namespace Fanex.Nap.CMS.CustomerClass.Models
{
    internal class SiteSetting
    {
        public SiteSetting(
            int policyId,
            string policyTitle,
            string site,
            string currencies,
            string currencyID,
            string platforms)
        {
            PolicyId = policyId;
            PolicyTitle = policyTitle;
            Site = site;
            Currencies = currencies;
            CurrencyID = currencyID;
            Platforms = platforms;
        }
        public int PolicyId { get; }

        public string PolicyTitle { get; }

        public string Site { get; }

        public string Currencies { get; }

        public string CurrencyID { get; }

        public bool IsAllCurrencies { get; set; }

        public string Platforms { get; }

        public string PlatformNames { get; set; }
    }
}
