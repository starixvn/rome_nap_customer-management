namespace Fanex.Nap.CMS.Customers.Criteria.LimitDailyPTUpdate
{
    using Fanex.Data.Repository;

    public class GetLimitDailyPTUpdateListCriteria : CriteriaBase
    {
        public GetLimitDailyPTUpdateListCriteria(
            string username,
            string site,
            string currency,
            string product,
            byte type,
            int pageIndex,
            int pageSize)
        {
            Username = username;
            Site = site;
            Currency = currency;
            Product = product;
            Type = type;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public string Username { get; }

        public string Site { get; }

        public string Currency { get; }

        public string Product { get; }

        public byte Type { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalItems { get; set; }

        //TODO: Update spname
        public override string GetSettingKey()
           => "";

        public override bool IsValid()
            => PageIndex > 0 && PageSize > 0;
    }
}
