using System.Collections.Generic;

namespace Fanex.Nap.CMS.TurnoverControl.Models.TurnoverMonitor
{
    public class MatchTurnover
    {
        public MatchTurnover(int matchId)
        {
            MatchId = matchId;
            CustomerTurnovers = new List<CustomerTurnover>();
        }

        public int MatchId { get; private set; }

        public int HomeId { get; set; }

        public string HomeName { get; set; }

        public int AwayId { get; set; }

        public string AwayName { get; set; }

        public bool IsNeutral { get; set; }

        public string EventDate { get; set; }

        public string EventStatus { get; set; }

        public IList<CustomerTurnover> CustomerTurnovers { get; private set; }
    }
}
