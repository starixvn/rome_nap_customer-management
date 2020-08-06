namespace Fanex.Nap.CMS.TurnoverControl.Models
{
    public class LockSettingParam
    {
        public LockSettingParam(
            int custId,
            bool isLock,
            bool isGroupHeadline,
            short sportType,
            int matchId = 0)
        {
            CustId = custId;
            IsLock = isLock;
            IsGroupHeadline = isGroupHeadline;
            SportType = sportType;
            MatchId = matchId;
        }

        public int CustId { get; }

        public bool IsLock { get; }

        public bool IsGroupHeadline { get; }

        public short SportType { get; }

        public int MatchId { get; }
    }
}
