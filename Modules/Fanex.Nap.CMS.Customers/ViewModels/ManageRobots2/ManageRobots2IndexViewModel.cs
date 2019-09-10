namespace Fanex.Nap.CMS.Customers.ViewModels.ManageRobots2
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Customers.Models;

    public class ManageRobots2IndexViewModel
    {
        public bool HasEditRight { get; set; }

        public IEnumerable<CurrencyModel> Currencies { get; set; }
    }
}
