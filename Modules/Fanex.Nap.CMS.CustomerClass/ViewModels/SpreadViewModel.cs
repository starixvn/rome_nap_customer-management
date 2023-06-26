namespace Fanex.Nap.CMS.CustomerClass.ViewModels
{
    using Fanex.Nap.CMS.Infrastructure.Models;
    using System.Collections.Generic;

    public class SpreadViewModel : BaseViewModel
    {

        public SpreadViewModel(bool hasHostPermission,
            IEnumerable<short> customerClasses,
            IEnumerable<IdNameModel> sites,
            IEnumerable<IdNameModel> currencies) : base(hasHostPermission)
        {
            CustomerClasses = customerClasses;
            Sites = sites;
            Currencies = currencies;
        }

        public IEnumerable<short> CustomerClasses { get; }
        public IEnumerable<IdNameModel> Sites { get; }
        public IEnumerable<IdNameModel> Currencies { get; }
    }
}
