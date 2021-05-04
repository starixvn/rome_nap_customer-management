using System.Collections.Generic;
using Fanex.Nap.CMS.Infrastructure.Models;

namespace Fanex.Nap.CMS.Customers.Models.VirtualRateEngine
{
    public class VREngineMatrix
    {
        public VREngineMatrix(
            IEnumerable<IdNameModel> creditSites,
            IEnumerable<IdNameModel> licenseeSites,
            IEnumerable<IdNameModel> customerClasses,
            IEnumerable<VREngineBetfrom> betfromList,
            IEnumerable<IdNameModel> sports,
            IEnumerable<IdNameModel> betTypes,
            IEnumerable<IdNameModel> leagueGroups)
        {
            CreditSites = creditSites;
            LicenseeSites = licenseeSites;
            CustomerClasses = customerClasses;
            BetfromList = betfromList;
            Sports = sports;
            BetTypes = betTypes;
            LeagueGroups = leagueGroups;
        }

        public IEnumerable<IdNameModel> CreditSites { get; }

        public IEnumerable<IdNameModel> LicenseeSites { get; }

        public IEnumerable<IdNameModel> CustomerClasses { get; }

        public IEnumerable<VREngineBetfrom> BetfromList { get; }

        public IEnumerable<IdNameModel> Sports { get; }

        public IEnumerable<IdNameModel> BetTypes { get; }

        public IEnumerable<IdNameModel> LeagueGroups { get; }
    }
}
