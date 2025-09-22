namespace Fanex.Nap.CMS.CustomerClass.Models
{
    using Fanex.Nap.CMS.Infrastructure.Models;
    using System;
    using System.Collections.Generic;

    public class SpreadSettingBodyExcelPreparer
    {
        public SpreadSettingBodyExcelPreparer() { }

        public SpreadSettingBodyExcelPreparer(
            int policyTitleMaxLength,
            IEnumerable<short> customerClasses,
            IEnumerable<IdNameModel> sportTypes,
            IEnumerable<SiteGroup> sites,
            IEnumerable<CurrencyModel> currencies,
            (decimal, decimal) spreadRange)
        {
            PolicyTitleMaxLength = policyTitleMaxLength;
            CustomerClasses.AddRange(customerClasses);
            SportTypes.AddRange(sportTypes);
            Sites.AddRange(sites);
            Currencies.AddRange(currencies);
            SpreadRange = spreadRange;
        }

        public int PolicyTitleMaxLength { get; } = 500;

        public List<short> CustomerClasses { get; } = new List<short>();

        public List<IdNameModel> SportTypes { get; } = new List<IdNameModel>();

        public List<SiteGroup> Sites { get; } = new List<SiteGroup>();

        public List<CurrencyModel> Currencies { get; } = new List<CurrencyModel>();

        public (decimal, decimal) SpreadRange { get; } = (0, 0.5M);
    }
}
