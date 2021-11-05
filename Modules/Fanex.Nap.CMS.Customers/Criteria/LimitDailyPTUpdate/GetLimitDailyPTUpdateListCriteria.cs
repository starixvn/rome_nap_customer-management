namespace Fanex.Nap.CMS.Customers.Criteria.LimitDailyPTUpdate
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Configurations;

    public class GetLimitDailyPTUpdateListCriteria : CriteriaBase
    {
        public GetLimitDailyPTUpdateListCriteria(
            string custName,
            string multiSite,
            string multiCurrency,
            byte productId,
            byte type,
            int pageIndex,
            int pageSize)
        {
            CustName = custName;
            MutilSite = multiSite;
            MutilCurrency = multiCurrency;
            ProductID = productId;
            Type = type;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public string CustName { get; }

        public string MutilSite { get; }

        public string MutilCurrency { get; }

        public byte ProductID { get; }

        public byte Type { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalItems { get; set; }

        public byte SettingStatus => AppSettings.SettingStatus;

        public override string GetSettingKey()
           => "NAP_LimitPTDaily_Sel";

        public override bool IsValid()
            => PageIndex > 0 && PageSize > 0;
    }
}
