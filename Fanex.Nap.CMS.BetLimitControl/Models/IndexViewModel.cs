namespace Fanex.NAP.CMS.BetLimitControl.Models
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public IEnumerable<CurrencyModel> Currencies { get; set; }

        public IEnumerable<SiteModel> Sites { get; set; }

        public IEnumerable<BetLimitTypeModel> BetLimitTypes { get; set; }
    }
}