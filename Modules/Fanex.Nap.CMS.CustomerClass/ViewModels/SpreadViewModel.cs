namespace Fanex.Nap.CMS.CustomerClass.ViewModels
{
    using Fanex.Nap.CMS.CustomerClass.Constants;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using System.Collections.Generic;
    using System.Linq;

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

        public IEnumerable<IdNameModel> Platforms { get; }
            = PlatformConst.Platforms.Where(key => key.Key != 0).Select(kvp => new IdNameModel { Id = kvp.Key, Name = kvp.Value }).ToList();
    }
}
