namespace Fanex.Nap.CMS.Customers.ViewModels.ManageRobots2
{
    using Fanex.Nap.CMS.Customers.Models;
    using System;
    using System.Collections.Generic;

    public class ManageRobots2IndexViewModel
    {
        public bool HasEditRight { get; set; }

        public IEnumerable<CurrencyModel> Currencies { get; set; }

        public DateTime CurrentDate { get; } = DateTime.Today;
    }
}