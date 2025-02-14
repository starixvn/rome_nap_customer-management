namespace Fanex.Nap.CMS.Customers.Criteria.LimitDailyPTUpdate
{
    using Fanex.Data.Repository;
    
    public class GetCurrencyListCriteria : CriteriaBase
    {
        public override string GetSettingKey()
           => "Acc_ExchangeSel";

        public override bool IsValid()
            => true;
    }
}
