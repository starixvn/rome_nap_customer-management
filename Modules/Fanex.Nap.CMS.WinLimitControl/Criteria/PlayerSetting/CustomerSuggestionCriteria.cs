namespace Fanex.Nap.CMS.WinLimitControl.Criteria.PlayerSetting
{
    using System;
    using Fanex.Data.Repository;
    using Fanex.NAP.Core.Configuration;

    public class CustomerSuggestionCriteria : CriteriaBase
    {
        public CustomerSuggestionCriteria(string customer)
        {
            CustomerName = customer;
        }

        [SpParam(SqlParamName = "custname")]
        public string CustomerName { get; set; }

        public bool ExcludeTestData
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ExcludeTestData"].Equals("1");
            }
        }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_CustSuggestionSel";
        }
    }
}
