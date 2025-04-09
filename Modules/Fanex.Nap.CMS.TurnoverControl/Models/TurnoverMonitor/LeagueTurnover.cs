using System.Collections.Generic;

namespace Fanex.Nap.CMS.TurnoverControl.Models.TurnoverMonitor
{
    public class LeagueTurnover
    {
        public LeagueTurnover()
        {
        }

        public LeagueTurnover(
            int leagueId,
            string leagueName,
            IEnumerable<MatchTurnover> matchTurnovers)
        {
            LeagueId = leagueId;
            LeagueName = leagueName;
            MatchTurnovers = matchTurnovers;
        }

        public int LeagueId { get; set; }

        public string LeagueName { get; set; }

        public IEnumerable<MatchTurnover> MatchTurnovers { get; set; }
    }
}
