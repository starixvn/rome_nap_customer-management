namespace Fanex.Nap.CMS.Customers.Criteria.LimitNewDownline
{
    public class RemoveLimitCriteria : AddNewLimitCriteria
    {
        public RemoveLimitCriteria(string usernames, int userId, string ip)
            : base(usernames, userId, ip)
        {
        }

        public override string GetSettingKey()
            => "Nap_ProblemAccountLimitDownlineDel";
    }
}
