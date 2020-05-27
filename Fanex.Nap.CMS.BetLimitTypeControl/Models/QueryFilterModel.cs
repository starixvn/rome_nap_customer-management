namespace Fanex.NAP.CMS.BetLimitTypeControl.Models
{
    using System.Collections.Generic;

    public class QueryFilterModel
    {
        public QueryFilterModel(IEnumerable<Product> products, IEnumerable<SportType> sportTypes, IEnumerable<BetType> betTypes)
        {
            Products = products;
            SportTypes = sportTypes;
            BetTypes = betTypes;
        }

        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<SportType> SportTypes { get; set; }

        public IEnumerable<BetType> BetTypes { get; set; }
    }
}
