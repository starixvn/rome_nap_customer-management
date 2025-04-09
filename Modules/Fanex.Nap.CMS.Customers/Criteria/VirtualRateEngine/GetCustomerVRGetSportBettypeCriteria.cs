namespace Fanex.Nap.CMS.Customers.Criteria.VirtualRateEngine
{
    using Fanex.Data.Repository;

    public class GetCustomerVRGetSportBettypeCriteria : CriteriaBase
    {
        public override string GetSettingKey()
            => "Acc_CustomerVRGetSportBettype";

        public override bool IsValid()
            => true;
    }
}
