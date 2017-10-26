using Fanex.NAP.Framework.Data;

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor
{
    public class GetHouseTurnoverCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int MatchId { get; set; }

        public bool IsGroupHeadline { get; set; }

        public string EventDate { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_TOControl_HouseTOByBettypeSelect";
        }

        public override bool IsValid()
        {
            bool validCustId = CustId > 0;
            bool validMatchId = MatchId > 0;
            bool validEventDate = !string.IsNullOrWhiteSpace(EventDate);

            return validCustId && validMatchId && validEventDate;
        }
    }
}
