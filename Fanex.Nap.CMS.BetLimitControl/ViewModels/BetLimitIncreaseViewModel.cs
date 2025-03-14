namespace Fanex.NAP.CMS.BetLimitControl.ViewModels
{
    using Fanex.Nap.CMS.Infrastructure.Models;
    using System.Collections.Generic;

    public class BetLimitIncreaseViewModel
    {
        public BetLimitIncreaseViewModel(IEnumerable<IdNameModel> sites, IEnumerable<IdNameModel> currencies, bool hasHostPermission)
        {
            Sites = sites;
            Currencies = currencies;
            HasHostPermission = hasHostPermission;
        }

        public IEnumerable<IdNameModel> Sites { get; }
        public IEnumerable<IdNameModel> Currencies { get; }
        public IEnumerable<IdNameModel> SportTypes { get; }
        public IEnumerable<IdNameModel> BetType { get; }
        public bool HasHostPermission { get; }
    }
}