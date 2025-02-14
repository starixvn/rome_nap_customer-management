namespace Fanex.Nap.CMS.TurnoverControl.Criteria
{
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class NonQueryCriteria : CommandBaseCriteria
    {
        public NonQueryCriteria(
            bool isGroupHeadline,
            short sportType,
            int userId,
            string ip)
        {
            IsGroupHeadline = isGroupHeadline;
            SportType = sportType;
            UserId = userId;
            IP = ip;
        }

        public bool IsGroupHeadline { get; }

        public short SportType { get; }

        public int UserId { get; }

        public string IP { get; }
    }
}
