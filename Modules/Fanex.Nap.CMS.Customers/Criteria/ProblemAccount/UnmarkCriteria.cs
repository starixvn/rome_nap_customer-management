using Fanex.Nap.CMS.Customers.Criteria.ProblemAccountLicenseeSite;

namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccount
{
    public class UnmarkCriteria : UnmarkLicenseeSiteCriteria
    {
        public UnmarkCriteria(
            string usernames,
            byte roleId,
            bool excludeAbnormalMember,
            bool excludeDirectMark,
            int userId,
            string ip) : base(usernames, userId, ip)
        {
            ExcludeAbnormalMember = excludeAbnormalMember;
            ExcludeDirectMark = excludeDirectMark;
            RoleId = roleId;
        }

        public bool ExcludeAbnormalMember { get; }

        public bool ExcludeDirectMark { get; }

        public byte RoleId { get; }

        public override string GetSettingKey()
            => "CS_ProblemAccount_RemoveCustomer";
    }
}
