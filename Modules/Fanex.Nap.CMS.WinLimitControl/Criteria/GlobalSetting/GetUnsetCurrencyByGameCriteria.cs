namespace Fanex.Nap.CMS.WinLimitControl.Criteria.GlobalSetting
{
    using Fanex.Data.Repository;

    public class GetUnsetCurrencyByGameCriteria : CriteriaBase
    {
        public GetUnsetCurrencyByGameCriteria(int gameId)
        {
            ProductId = gameId;
        }

        public int ProductId { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_NotConfigCurrencySel";
        }
    }
}
