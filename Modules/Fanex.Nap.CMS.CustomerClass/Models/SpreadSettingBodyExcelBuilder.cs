using Fanex.Nap.CMS.CustomerClass.Constants;
using Fanex.Nap.CMS.Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Nap.CMS.CustomerClass.Models
{
    internal class SpreadSettingBodyExcelBuilder
    {
        public SpreadSettingBodyExcelBuilder(SpreadSettingBodyExcelPreparer data)
        {
            PolicyTitleMaxLength = data.PolicyTitleMaxLength;
            CustomerClassesHashSet = new HashSet<short>(data.CustomerClasses);
            SportTypesDictionary = data.SportTypes.ToDictionary(kvp => kvp.Name, kvp => kvp.Id);
            SitesDictionary = data.Sites.ToDictionary(kvp => kvp.Name, kvp => kvp.Id);
            CurrenciesDictionary = data.Currencies.ToDictionary(kvp => kvp.Currency, kvp => kvp.ExchangeId);
            PlatformsDictionary = PlatformConst.Platforms.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
            SpreadRange = data.SpreadRange;
        }

        public int PolicyTitleMaxLength { get; }

        public HashSet<short> CustomerClassesHashSet { get; }

        public IDictionary<string, int> SportTypesDictionary { get; }

        public IDictionary<string, int> SitesDictionary { get; }

        public IDictionary<string, int> CurrenciesDictionary { get; }

        public IDictionary<string, byte> PlatformsDictionary { get; }

        public (decimal, decimal) SpreadRange { get; }
    }
}
