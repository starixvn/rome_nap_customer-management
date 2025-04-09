namespace Fanex.Nap.CMS.Settings.Criteria
{
    using Fanex.Data.Repository;

    public class GetGameCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "NAP_WinLimit_ProductSel";
        }

        public override bool IsValid()
            => true;
    }
}
