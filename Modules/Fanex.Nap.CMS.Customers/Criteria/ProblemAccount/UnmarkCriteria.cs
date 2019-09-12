namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccount
{
    public class UnmarkCriteria : NonQueryCriteriaBase
    {
        public UnmarkCriteria(
            string usernames,
            byte roleId,
            bool excludeAbnormalMember,
            bool excludeDirectMark,
            int userId,
            string ip) : base(usernames, roleId, userId, ip)
        {
            ExcludeAbnormalMember = excludeAbnormalMember;
            ExcludeDirectMark = excludeDirectMark;
        }

        public bool ExcludeAbnormalMember { get; }

        public bool ExcludeDirectMark { get; }

        public override string GetSettingKey()
            => "CS_ProblemAccount_RemoveCustomer";
    }
}
