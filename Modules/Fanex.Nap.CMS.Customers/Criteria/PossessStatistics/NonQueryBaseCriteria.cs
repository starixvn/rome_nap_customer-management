namespace Fanex.Nap.CMS.Customers.Criteria.PossessStatistics
{
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class NonQueryBaseCriteria : CommandBaseCriteria
    {
        public NonQueryBaseCriteria(int userId, string ip)
        {
            UserId = userId;
            IP = ip;
        }

        public int UserId { get; }

        public string IP { get; }

        public override string GetSettingKey()
        {
            throw new System.NotImplementedException();
        }

        public override bool IsValid()
            => true;
    }
}