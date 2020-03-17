namespace Fanex.Nap.CMS.Customers.ViewModels.ManageRobots2
{
    using Fanex.Nap.CMS.Customers.Models;
    using System;
    using System.Collections.Generic;

    public class ManageRobots2IndexViewModel
    {
        // The release date of this feature.
        private static readonly DateTime DefaultMinDate = new DateTime(2020, 3, 17);

        public ManageRobots2IndexViewModel()
        {
            MinFromDate = DefaultMinDate;
        }

        public bool HasEditRight { get; set; }

        public IEnumerable<CurrencyModel> Currencies { get; set; }

        public DateTime MinFromDate { get; }

        public DateTime CurrentDate { get; } = DateTime.Today;
    }
}