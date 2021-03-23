using Fanex.Data.Repository;

namespace Fanex.Nap.CMS.Customers.Criteria.LimitNewDownline
{
    public class GetLimitDownlineListCriteria : CriteriaBase
    {
        public GetLimitDownlineListCriteria(
            string username,
            byte level,
            string site,
            int pageIndex,
            int pageSize)
        {
            Username = username;
            Level = level;
            Site = site;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public string Username { get; }

        public byte Level { get; }

        public string Site { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalItems { get; set; }

        public override string GetSettingKey()
            => "Nap_ProblemAccountLimitDownlineSel";

        public override bool IsValid()
            => PageIndex > 0 && PageSize > 0;
    }
}
