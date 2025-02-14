namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails
{
    using Fanex.Data.Repository;

    public class ViewRealValuesCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int SportType { get; set; }

        public int ErrCode { get; set; } = 0;

        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey()
            => "Acc_CustRealMultiplier_Sel";

        public override bool IsValid()
            => true;
    }
}
