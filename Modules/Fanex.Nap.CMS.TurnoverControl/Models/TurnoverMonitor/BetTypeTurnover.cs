namespace Fanex.Nap.CMS.TurnoverControl.Models.TurnoverMonitor
{
    public class BetTypeTurnover
    {
        public BetTypeTurnover(TurnoverMonitorItem item)
        {
            ProfileId = item.ProfileId;
            MaxTurnover = item.MaxTurnover;
            PercentageUsed = item.PercentageUsed;
            HouseTurnover = item.HouseTurnover;
        }

        public int ProfileId { get; }

        public decimal? MaxTurnover { get; }

        public decimal? PercentageUsed { get; }

        public decimal? HouseTurnover { get; }
    }
}
