namespace Fanex.Nap.CMS.CustomerClass.Models
{
    internal class SiteSetting
    {
        public int PolicyId { get; set; }

        public string PolicyTitle { get; set; }

        public string Site { get; set; }

        public string Currencies { get; set; }

        public string CurrencyID { get; set; }

        public bool IsAllCurrencies { get; set; }

        public string Platforms { get; set; }

        public string PlatformNames { get; set; }
    }
}
