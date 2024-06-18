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

        public string Sites { get; }
        public int NumberOfSites { get; }
        public string CurrencyName { get; }
        public int NumberOfCurrencies { get; }
        public string Platform { get; }
        public string PlatformNames { get; set; }
        public int NumberOfPlatform { get; }
    }
}
