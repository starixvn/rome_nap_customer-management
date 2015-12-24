namespace Fanex.Nap.CMS.Settings.Criteria
{
    using Fanex.Data.Repository;

    public class GetCurrencyCriteria : CriteriaBase
    {
        public bool ExcludeTestData
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ExcludeTestData"].Equals("1");
            }
        }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_CurrencySel";
        }
    }
}
