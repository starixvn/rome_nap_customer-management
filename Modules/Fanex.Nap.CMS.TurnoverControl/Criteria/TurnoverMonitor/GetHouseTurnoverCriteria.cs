using Fanex.NAP.Framework.Data;

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor
{
    public class GetHouseTurnoverCriteria : CriteriaBase
    {
        public GetHouseTurnoverCriteria(int custId, int matchId, bool isGroupHeadline, string eventDate)
        {
            CustId = custId;
            MatchId = matchId;
            IsGroupHeadline = isGroupHeadline;
            EventDate = eventDate;
        }

        public int CustId { get; }

        public int MatchId { get; }

        public bool IsGroupHeadline { get; }

        public string EventDate { get; }

        public override string GetSettingKey()
            => "NAP_TOControl_HouseTOByBettypeSelect";

        public override bool IsValid()
            => CustId > 0 && MatchId > 0 && !string.IsNullOrWhiteSpace(EventDate);
    }
}
