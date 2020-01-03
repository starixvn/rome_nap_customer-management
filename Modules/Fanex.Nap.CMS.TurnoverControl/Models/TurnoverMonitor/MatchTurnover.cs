using System.Collections.Generic;

namespace Fanex.Nap.CMS.TurnoverControl.Models.TurnoverMonitor
{
    public class MatchTurnover
    {
        public MatchTurnover(
            int matchId,
            string matchName,
            string eventDate,
            string eventStatus)
        {
            MatchId = matchId;
            MatchName = matchName;
            EventDate = eventDate;
            EventStatus = eventStatus;
            CustomerTurnovers = new List<CustomerTurnover>();
        }

        public int MatchId { get; }

        public string MatchName { get; }

        public string EventDate { get; }

        public string EventStatus { get; }

        public IList<CustomerTurnover> CustomerTurnovers { get; }
    }
}
