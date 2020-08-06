using Fanex.Nap.CMS.TurnoverControl.Models;

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor
{
    public class LockEventSettingCriteria : NonQueryCriteria
    {
        public LockEventSettingCriteria(LockSettingParam param, int userId, string ip)
            : base(param.IsGroupHeadline, param.SportType, userId, ip)
        {
            CustId = param.CustId;
            MatchId = param.MatchId;
            IsEventLockTOLimit = param.IsLock;
        }

        public int CustId { get; }

        public int MatchId { get; }

        public bool IsEventLockTOLimit { get; }

        public override string GetSettingKey()
            => "NAP_TOControl_MatchTurnoverLockTOLimit_Update";
    }
}
