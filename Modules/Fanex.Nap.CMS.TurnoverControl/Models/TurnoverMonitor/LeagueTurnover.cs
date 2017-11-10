using System.Collections.Generic;

namespace Fanex.Nap.CMS.TurnoverControl.Models.TurnoverMonitor
{
    public class LeagueTurnover
    {
        public LeagueTurnover(int leagueId, string leagueName)
        {
            LeagueId = leagueId;
            LeagueName = leagueName;
            MatchTurnovers = new List<MatchTurnover>();
        }

        public int LeagueId { get; private set; }

        public string LeagueName { get; private set; }

        public IList<MatchTurnover> MatchTurnovers { get; private set; }
    }
}
