namespace Fanex.Nap.CMS.TurnoverControl.Models.TurnoverMonitor
{
    public sealed class UpdateMaxTurnoverViewModel
    {
        public int CustId { get; set; }

        public int MatchId { get; set; }

        public bool IsGroupHeadline { get; set; }

        public decimal MaxTurnover { get; set; }

        public short SportType { get; set; }
    }
}
