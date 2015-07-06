namespace Fanex.Nap.CMS.WinLimitControl.Criteria.CurrencySetting
{
    using Fanex.Data.Repository;

    public class GetExcludedGameCriteria : CriteriaBase
    {
        public bool ExcludeUsed
        {
            get { return true; }
        }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_ProductSel";
        }
    }
}
