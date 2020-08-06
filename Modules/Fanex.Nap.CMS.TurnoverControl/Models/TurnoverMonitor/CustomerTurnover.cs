using System;
using System.Collections.Generic;

namespace Fanex.Nap.CMS.TurnoverControl.Models.TurnoverMonitor
{
    public class CustomerTurnover
    {
        public CustomerTurnover(TurnoverMonitorItem item)
        {
            SportType = item.SportType;
            MatchId = item.MatchId;
            EventStatus = item.EventStatus;
            CustId = item.CustId;
            Username = item.Username;
            Level = item.Level;
            IsGlobalLock = item.IsGlobalLockTOLimit;
            IsEventLock = item.IsEventLockTOLimit;
            Currency = item.Currency;
            TotalMemberTurnover = item.TotalMemberTurnover;
            TotalHouseTurnover = item.TotalHouseTurnover;
            HDPML = new List<BetTypeTurnover>();
            OU = new List<BetTypeTurnover>();
            _1HHDP1HML = new List<BetTypeTurnover>();
            _1HOU = new List<BetTypeTurnover>();
            Others = new List<BetTypeTurnover>();
        }

        public short SportType { get; }

        public int MatchId { get; }

        public string EventStatus { get; }

        public int CustId { get; }

        public string Username { get; }

        public string Level { get; }

        public bool IsGlobalLock { get; }

        public bool IsEventLock { get; }

        public bool IsGroupHeadline
            => Level.Equals("headline", StringComparison.OrdinalIgnoreCase);

        public string Currency { get; }

        public decimal TotalMemberTurnover { get; }

        public decimal TotalHouseTurnover { get; }

        public IList<BetTypeTurnover> HDPML { get; }

        public IList<BetTypeTurnover> OU { get; }

        public IList<BetTypeTurnover> _1HHDP1HML { get; }

        public IList<BetTypeTurnover> _1HOU { get; }

        public IList<BetTypeTurnover> Others { get; }
    }
}
