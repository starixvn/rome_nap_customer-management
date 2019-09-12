namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccount
{
    public class RemoveReduceBLCriteria : InactivateCriteria
    {
        public RemoveReduceBLCriteria(
            string usernames,
            byte roleId,
            bool isAllDownlines,
            bool confirmed,
            int userId,
            string ip) : base(usernames, roleId, confirmed, userId, ip)
        {
            isAllDownline = isAllDownlines;
        }

        public bool isAllDownline { get; }

        public override string GetSettingKey()
            => "CS_ProblemAccount_RemoveReduceBetLimit";
    }
}
