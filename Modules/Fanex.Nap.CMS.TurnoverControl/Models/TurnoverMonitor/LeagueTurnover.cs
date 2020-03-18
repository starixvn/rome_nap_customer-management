using System.Collections.Generic;

namespace Fanex.Nap.CMS.TurnoverControl.Models.TurnoverMonitor
{
    public class LeagueTurnover
    {
        public LeagueTurnover(
            int leagueId,
            string leagueName,
            IEnumerable<MatchTurnover> matchTurnovers)
        {
            LeagueId = leagueId;
            LeagueName = leagueName;
            MatchTurnovers = matchTurnovers;
        }

        public int LeagueId { get; }

        public string LeagueName { get; }

        public IEnumerable<MatchTurnover> MatchTurnovers { get; }
    }
}
