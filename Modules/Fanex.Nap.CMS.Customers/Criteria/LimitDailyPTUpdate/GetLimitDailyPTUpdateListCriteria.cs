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
            string productId,
            byte type,
            int pageIndex,
            int pageSize,
            byte isLimitUpdPT)
        {
            CustName = custName;
            MutilSite = multiSite;
            MutilCurrency = multiCurrency;
            ProductID = productId;
            Type = type;
            PageIndex = pageIndex;
            PageSize = pageSize;
            IsLimitUpdPT = isLimitUpdPT;
        }

        public string CustName { get; }

        public string MutilSite { get; }

        public string MutilCurrency { get; }

        public string ProductID { get; }

        public byte Type { get; }

        public int PageIndex { get; }

        public int PageSize { get; }
        public byte IsLimitUpdPT { get; }

        public int TotalItems { get; set; }

        public byte SettingStatus => AppSettings.SettingStatus;

        public override string GetSettingKey()
           => "NAP_LimitPTDaily_Sel";

        public override bool IsValid()
            => PageIndex > 0 && PageSize > 0;
    }
}
