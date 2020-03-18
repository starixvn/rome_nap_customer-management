namespace Fanex.Nap.CMS.TurnoverControl.ViewModels.TurnoverMonitor
{
    public class CustomerTurnoverViewModel
    {
        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public short SportType { get; set; }

        public int LeagueId { get; set; }

        public int MatchId { get; set; }

        public string Username { get; set; }

        public bool IsExcludeCompletedEvent { get; set; }

        public int PercentageUsed { get; set; }
    }
}
