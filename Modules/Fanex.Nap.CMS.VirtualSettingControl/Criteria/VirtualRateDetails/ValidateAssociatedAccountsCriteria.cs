namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails
{
    using Fanex.Data.Repository;

    public class ValidateAssociatedAccountsCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "UsernameLst")]
        public string Accounts { get; set; }

        public override string GetSettingKey()
            => "Acc_CustVirtualMultiplierInvalid_Check";
    }
}
