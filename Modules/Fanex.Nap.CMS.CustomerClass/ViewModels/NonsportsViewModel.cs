namespace Fanex.Nap.CMS.CustomerClass.ViewModels
{
    using Fanex.Nap.CMS.Infrastructure.Models;
    using System.Collections.Generic;

    public class NonsportsViewModel : BaseViewModel
    {
        public NonsportsViewModel(bool hasHostPermission,
              IEnumerable<short> customerClasses,
              IEnumerable<IdNameModel> sites,
              IEnumerable<IdNameModel> products) : base(hasHostPermission)
        {
            CustomerClasses = customerClasses;
            Sites = sites;
            Products = products;
        }
        public IEnumerable<short> CustomerClasses { get; }

        public IEnumerable<IdNameModel> Sites { get; }

        public IEnumerable<IdNameModel> Products { get; }
    }
}
