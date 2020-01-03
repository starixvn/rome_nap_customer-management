namespace Fanex.Nap.CMS.TurnoverControl.ViewModels.TurnoverMonitor
{
    public class HouseTurnoverViewModel
    {
        public int CustId { get; set; }

        public int MatchId { get; set; }

        public bool IsGroupHeadline { get; set; }

        public string EventDate { get; set; }

        public bool IsValid()
            => CustId > 0 && MatchId > 0 && !string.IsNullOrWhiteSpace(EventDate);
    }
}
