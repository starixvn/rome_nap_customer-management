namespace Fanex.Nap.CMS.Customers.Criteria.LimitDailyPTUpdate
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Configurations;

    public class GetProductListCriteria : CriteriaBase
    {
        public byte SettingStatus => AppSettings.SettingStatus;

        public override string GetSettingKey()
            => "NAP_LimitPTDaily_ProductList";

        public override bool IsValid()
            => true;
    }
}
