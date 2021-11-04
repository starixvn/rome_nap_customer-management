namespace Fanex.Nap.CMS.Customers.Criteria.LimitDailyPTUpdate
{
    using Fanex.Data.Repository;

    public class GetProductListCriteria : CriteriaBase
    {
        public GetProductListCriteria(int settingStatus)
        {
            SettingStatus = settingStatus;
        }

        public int SettingStatus { get; }

        public override string GetSettingKey()
            => "NAP_LimitPTDaily_ProductList";
    }
}
