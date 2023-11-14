namespace Fanex.Nap.CMS.Infrastructure.Criteria
{
    using Fanex.Data.Repository;

    internal class GetNAPAccPolicyCriteria : CriteriaBase
    {
        public GetNAPAccPolicyCriteria(int custId)
            => CustId = custId;

        public int CustId { get; }

        public override string GetSettingKey()
            => "Acc_Con_FileNameSelByCustID";
    }
}
