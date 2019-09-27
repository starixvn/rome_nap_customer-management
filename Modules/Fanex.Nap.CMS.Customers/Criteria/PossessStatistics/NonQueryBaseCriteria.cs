namespace Fanex.Nap.CMS.Customers.Criteria.PossessStatistics
{
    using Fanex.Data.Repository;

    public class NonQueryBaseCriteria : CriteriaBase
    {
        public NonQueryBaseCriteria(int userId, string ip)
        {
            UserId = userId;
            IP = ip;
        }

        public int UserId { get; }

        public string IP { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}