namespace Fanex.Nap.CMS.TurnoverControl.Models
{
    public sealed class UpdateMaxTurnoverParam
    {
        public UpdateMaxTurnoverParam(
            int custId,
            decimal maxTurnover,
            bool isGroupHeadline,
            short sportType,
            int profileId,
            int matchId = 0)
        {
            CustId = custId;
            MaxTurnover = maxTurnover;
            IsGroupHeadline = isGroupHeadline;
            SportType = sportType;
            ProfileId = profileId;
            MatchId = matchId;
        }

        public int CustId { get; }

        public int ProfileId { get; }

        public decimal MaxTurnover { get; }

        public bool IsGroupHeadline { get; }

        public short SportType { get; }

        public int MatchId { get; }
    }
}
