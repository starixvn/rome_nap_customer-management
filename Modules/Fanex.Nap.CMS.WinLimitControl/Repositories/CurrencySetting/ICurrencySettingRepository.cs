namespace Fanex.Nap.CMS.WinLimitControl.Repositories.CurrencySetting
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Settings.Models;
    using Fanex.Nap.CMS.WinLimitControl.Models.CurrencySetting;

    public interface ICurrencySettingRepository
    {
        IList<WinLimitEntity> GetGlobalWinLimits(ICriteria criteria);

        IList<BaseSetting> GetExcludedCurrencies(ICriteria criteria);

        IList<BaseSetting> GetExcludedGames(ICriteria criteria);

        IList<BaseSetting> GetUnsetCurrencies(ICriteria criteria);

        IList<BaseSetting> GetUnsetGames(ICriteria criteria);

        Result AddGlobalWinLimit(ICriteria criteria);

        IList<GreaterPlayerWinLimit> GetGreaterPlayerWinLimits(ICriteria criteria);

        Result UpdateGlobalWinLimit(ICriteria criteria);
    }
}
