namespace Fanex.NAP.CMS.BetLimitControl.ViewModels
{
    using Fanex.Nap.CMS.Infrastructure.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class BetLimitIncreaseViewModel
    {
        public BetLimitIncreaseViewModel(IEnumerable<IdNameModel> sites, IEnumerable<CurrencyModel> currencies, bool hasHostPermission)
        {
            Sites = sites;
            Currencies = currencies.Select(c => new IdNameModel { Id = c.ExchangeId, Name = c.Currency });
            HasHostPermission = hasHostPermission;
        }

        public IEnumerable<IdNameModel> Sites { get; }
        public IEnumerable<IdNameModel> Currencies { get; }
        public IEnumerable<IdNameModel> SportTypes { get; }
        public IEnumerable<IdNameModel> BetType { get; }
        public bool HasHostPermission { get; }
    }
}