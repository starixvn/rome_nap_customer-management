using Fanex.Data.Repository;

namespace Fanex.Nap.CMS.Customers.Criteria.LimitNewDownline
{
    public class GetCreditSiteCriteria : CriteriaBase
    {
        public GetCreditSiteCriteria(int mode)
        {
            Mode = mode;
        }

        public int Mode { get; }

        public override string GetSettingKey()
            => "Acc_SiteSel_Credit";

        public override bool IsValid()
            => true;
    }
}
