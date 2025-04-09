namespace Fanex.Nap.CMS.Customers.ViewModels.ManageRobots2
{
    using System;
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public class ManageRobots2IndexViewModel
    {
        public bool HasEditRight { get; set; }

        public IEnumerable<CurrencyModel> Currencies { get; set; }

        public DateTime CurrentDate { get; } = DateTime.Today;
    }
}