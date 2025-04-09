namespace Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor
{
    public class LockEventSettingCriteria : NonQueryCriteria
    {
        public int CustId { get; set; }

        public int MatchId { get; set; }

        public bool IsEventLockTOLimit { get; set; }

        public override string GetSettingKey()
            => "NAP_TOControl_MatchTurnoverLockTOLimit_Update";
    }
}
