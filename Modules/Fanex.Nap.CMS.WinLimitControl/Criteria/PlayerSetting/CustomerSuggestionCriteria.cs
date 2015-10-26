namespace Fanex.Nap.CMS.WinLimitControl.Criteria.PlayerSetting
{
    using Fanex.Data.Repository;

    public class CustomerSuggestionCriteria : CriteriaBase
    {
        public CustomerSuggestionCriteria(string customer)
        {
            CustomerName = customer;
        }

        [SpParam(SqlParamName = "custname")]
        public string CustomerName { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_CustSuggestionSel";
        }
    }
}
