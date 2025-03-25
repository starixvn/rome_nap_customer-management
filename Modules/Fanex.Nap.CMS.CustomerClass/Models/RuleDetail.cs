namespace Fanex.Nap.CMS.CustomerClass.Models
{
    internal class RuleDetail
    {
        public RuleDetail(string sites, int numberOfSites, string currencyName, int numberOfCurrencies, string platform, int numberOfPlatform)
        {
            Sites = string.Join(", ", sites.Split(','));
            NumberOfSites = numberOfSites;
            CurrencyName = string.Join(", ", currencyName.Split(','));
            NumberOfCurrencies = numberOfCurrencies;
            Platform = platform;
            NumberOfPlatform = numberOfPlatform;
        }

        public string Sites { get; set; }
        public int NumberOfSites { get; set; }
        public string CurrencyName { get; set; }
        public int NumberOfCurrencies { get; set; }
        public string Platform { get; set; }
        public string PlatformNames { get; set; }
        public int NumberOfPlatform { get; set; }
    }
}
