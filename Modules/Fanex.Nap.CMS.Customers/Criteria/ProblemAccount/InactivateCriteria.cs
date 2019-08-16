namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccount
{
    public class InactivateCriteria : NonQueryCriteriaBase
    {
        public InactivateCriteria(
            string usernames,
            byte roleId,
            bool confirmed,
            int userId,
            string ip) : base(usernames, roleId, userId, ip)
        {
            IsConfirmed = confirmed;
        }

        public bool IsConfirmed { get; }

        public override string GetSettingKey()
            => "CS_ProblemAccount_InactiveAccount";
    }
}
