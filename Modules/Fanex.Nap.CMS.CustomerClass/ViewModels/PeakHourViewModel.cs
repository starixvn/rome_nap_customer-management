using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Nap.CMS.CustomerClass.ViewModels
{
    using Fanex.Nap.CMS.CustomerClass.Constants;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    public class PeakHourViewModel : BaseViewModel
    {

        public PeakHourViewModel(bool hasHostPermission,
            IEnumerable<short> customerClasses,
            IEnumerable<IdNameModel> sites,
            IEnumerable<IdNameModel> currencies,
            IEnumerable<IdNameModel> sportTypes,
            IEnumerable<LeagueGroup> leagueGroups) : base(hasHostPermission)
        {
            CustomerClasses = customerClasses;
            Sites = sites;
            Currencies = currencies;
            SportTypes = sportTypes.Select(c => ToSelectListItem(c));
            LeagueGroups = leagueGroups;
        }
        public IEnumerable<short> CustomerClasses { get; }

        public IEnumerable<IdNameModel> Sites { get; }

        public IEnumerable<IdNameModel> Currencies { get; }

        public IEnumerable<SelectListItem> SportTypes { get; }

        public IEnumerable<LeagueGroup> LeagueGroups { get; }

        public IEnumerable<IdNameModel> Platforms { get; }
            = PlatformConst.Platforms.Where(key => key.Key != 0).Select(kvp => new IdNameModel { Id = kvp.Key, Name = kvp.Value }).ToList();
    }
}
