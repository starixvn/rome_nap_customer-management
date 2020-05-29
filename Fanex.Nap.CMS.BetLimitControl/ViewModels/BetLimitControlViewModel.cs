using System.Collections.Generic;
using Fanex.Nap.CMS.Customers.Models;

namespace Fanex.Nap.CMS.BetLimitControl.ViewModels
{
    public class BetLimitControlViewModel
    {
        public bool HasEditRight { get; set; }

        public IEnumerable<CurrencyModel> Currencies { get; set; }
    }
}