using System.Collections.Generic;
using System.Linq;

namespace Fanex.Nap.CMS.BetLimitControl.Models
{
    public class SearchModel
    {
        public int TotalItems { get; set; }

        public IEnumerable<IGrouping<string, CustomerBetLimitModel>> CustomerBetLimitControls { get; set; }
    }
}