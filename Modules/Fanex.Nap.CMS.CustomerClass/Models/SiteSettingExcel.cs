namespace Fanex.Nap.CMS.CustomerClass.Models
{
    using System.Collections.Generic;

    public class SiteSettingExcel
    {
        public SiteSettingExcel() { }
        public SiteSettingExcel(
            int policyId,
            IEnumerable<string> sites,
            IEnumerable<int> currencies,
            IEnumerable<byte> platforms)
        {
            PolicyId = policyId;
            Sites.AddRange(sites);
            Currencies.AddRange(currencies);
            Platforms.AddRange(platforms);
        }

        public int PolicyId { get; set; }

        public List<string> Sites { get; } = new List<string>();

        public List<int> Currencies { get; } = new List<int>();

        public List<byte> Platforms { get; } = new List<byte>();
    }
}
