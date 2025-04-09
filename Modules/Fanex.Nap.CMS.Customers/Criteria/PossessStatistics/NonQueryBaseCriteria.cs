namespace Fanex.Nap.CMS.Customers.Criteria.PossessStatistics
{
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class NonQueryBaseCriteria : CommandBaseCriteria
    {
        public int UserId { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            throw new System.NotImplementedException();
        }

        public override bool IsValid()
            => true;
    }
}