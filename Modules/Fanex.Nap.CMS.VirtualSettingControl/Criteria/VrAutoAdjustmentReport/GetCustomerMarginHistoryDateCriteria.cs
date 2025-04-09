namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VrAutoAdjustmentReport
{
    using Fanex.Data.Repository;

    public class GetCustomerMarginHistoryDateCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "NAP_Report_CustMngt_GetCustomerMarginHistoryDate";
        }

        public override bool IsValid()
            => true;
    }
}