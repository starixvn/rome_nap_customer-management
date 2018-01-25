namespace Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor
{
    public class UpdateMaxTurnoverCriteria : NonQueryCriteria
    {
        public int CustId { get; set; }

        public int MatchId { get; set; }

        public short SportType { get; set; }

        public bool IsGroupHeadline { get; set; }

        public decimal MaxTurnover { get; set; }

        public override string GetSettingKey()
        {
            return "TurnoverControl_TurnoverMonitor_UpdateByMatch";
        }

        public override bool IsValid()
        {
            bool validCustId = CustId > 0;
            bool validMatchId = MatchId > 0;
            bool validSportType = SportType > 0;
            bool validMaxTurnover = 0 <= MaxTurnover && MaxTurnover <= int.MaxValue;

            return validCustId && validMatchId && validSportType && validMaxTurnover;
        }
    }
}
