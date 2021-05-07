using Fanex.Data.Repository;

namespace Fanex.Nap.CMS.Customers.Criteria.VirtualRateEngine
{
    internal class VREngineCriteriaBase : CriteriaBase
    {
        protected VREngineCriteriaBase(int userId, string ip)
        {
            UserId = userId;
            IP = ip;
        }

        public int UserId { get; }

        public string IP { get; }

        public int ErrorCode { get; set; }

        public string ErrorMsg { get; set; }

        public override bool IsValid()
            => UserId > 0;
    }
}
