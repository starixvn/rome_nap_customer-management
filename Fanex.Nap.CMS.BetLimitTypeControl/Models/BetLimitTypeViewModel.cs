using System.Collections.Generic;
using System.Linq;

namespace Fanex.NAP.CMS.BetLimitTypeControl.Models
{
    public class BetLimitTypeViewModel
    {
        private readonly IEnumerable<LeagueGroup> AllSabaLeagueGroups = new List<LeagueGroup>
        {
            new LeagueGroup(42, "Virtual", 0),
            new LeagueGroup(74, "Virtual Sport Bacarrat", 1),
            new LeagueGroup(108, "Virtual Futsal", 1),
            new LeagueGroup(113, "SABA PinGoal", 1),
            new LeagueGroup(122, "SABA Basketball PinGoal", 2),
            new LeagueGroup(151, "SABA E-Sports PinGoal", 43)
        };

        public BetLimitTypeViewModel(IEnumerable<LeagueGroup> leagueGroups)
        {
            SabaSportsLeagueGroups = leagueGroups.Where(lg => AllSabaLeagueGroups.Any(sblg => lg.Id == sblg.Id)).OrderBy(lg => lg.Id);
            OtherLeagueGroups = leagueGroups.Where(lg => !AllSabaLeagueGroups.Any(sblg => lg.Id == sblg.Id));
        }

        public IEnumerable<LeagueGroup> SabaSportsLeagueGroups { get; set; }
        public IEnumerable<LeagueGroup> OtherLeagueGroups { get; set; }
    }
}
