using Fanex.Data.Repository;

namespace Fanex.Nap.CMS.PayoutManagement.Criteria
{
    public class GetSiteIdByCustomerIdCriteria : CriteriaBase
    {
        public GetSiteIdByCustomerIdCriteria(int customerId)
        {
            CustomerId = customerId;
        }

        [SpParam(SqlParamName = "custid")]
        public int CustomerId { get; }

        [SpParam(SqlParamName = "siteid")]
        public int SiteId { get; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerSelSiteByCustId";
        }

        public override bool IsValid()
        {
            return CustomerId > 0;
        }
    }
}
