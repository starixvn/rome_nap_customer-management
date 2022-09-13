namespace Fanex.Nap.CMS.Customers.Criteria.ManageFollowBetAccount
{
    using Fanex.Data.Repository;

    public class GetCreditSiteCriteria : CriteriaBase
    {
        public GetCreditSiteCriteria(int mode)
        {
            Mode = mode;
        }

        public int Mode { get; }

        public override string GetSettingKey()
            => "Acc_SiteSel_Credit";
    }
}
