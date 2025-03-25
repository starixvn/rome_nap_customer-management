namespace Fanex.Nap.CMS.CustomerClass.ViewModels
{
    using Fanex.Nap.CMS.Infrastructure.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class UtilityOfferingViewModel : BaseViewModel
    {
        public UtilityOfferingViewModel(bool hasHostPermission,
              IEnumerable<short> customerClasses,
              IEnumerable<IdNameModel> sites,
              IEnumerable<CurrencyModel> currencies) : base(hasHostPermission)
        {
            CustomerClasses = customerClasses;
            Sites = sites;
            Currencies = currencies.Select(c => new IdNameModel { Id = c.ExchangeId, Name = c.Currency });
        }
        public IEnumerable<short> CustomerClasses { get; }

        public IEnumerable<IdNameModel> Sites { get; }

        public IEnumerable<IdNameModel> Currencies { get; }
    }
}
