using Fanex.Nap.CMS.CustomerClass.Constants;
using Fanex.Nap.CMS.CustomerClass.Resources;
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
            SpreadRange = data.SpreadRange;

            SportTypesDictionary = data.SportTypes.ToDictionary(kvp => kvp.Name, kvp => kvp.Id);
            SportTypesDictionary.Add(Labels.All, 0);

            SitesDictionary = data.Sites.ToDictionary(kvp => kvp.Name, kvp => kvp.Id);
            SitesDictionary.Add(Labels.All, 0);
            SitesDictionary.Add(Labels.AllCredit, -1);
            SitesDictionary.Add(Labels.AllDeposit, -2);

            CurrenciesDictionary = data.Currencies.ToDictionary(kvp => kvp.Currency, kvp => kvp.ExchangeId);
            CurrenciesDictionary.Add(Labels.All, 0);

            PlatformsDictionary = PlatformConst.Platforms.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
            PlatformsDictionary.Add(Labels.All, 0);
        }

        public int PolicyTitleMaxLength { get; }

        /// <summary>
        /// All: is all customerClass
        /// </summary>
        public HashSet<short> CustomerClassesHashSet { get; }

        public IDictionary<string, int> SportTypesDictionary { get; }

        /// <summary>
        /// 0: All
        /// -1: All Credit
        /// -2: All Deposit
        /// </summary>
        public IDictionary<string, int> SitesDictionary { get; }

        /// <summary>
        /// 0: All
        /// </summary>
        public IDictionary<string, int> CurrenciesDictionary { get; }

        /// <summary>
        /// 0: All
        /// </summary>
        public IDictionary<string, byte> PlatformsDictionary { get; }

        public (decimal, decimal) SpreadRange { get; }
    }
}
