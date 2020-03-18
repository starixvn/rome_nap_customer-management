using Fanex.NAP.Framework.Data;

namespace Fanex.Nap.CMS.TurnoverControl.Criteria
{
    public class NonQueryCriteria : CriteriaBase
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

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}
