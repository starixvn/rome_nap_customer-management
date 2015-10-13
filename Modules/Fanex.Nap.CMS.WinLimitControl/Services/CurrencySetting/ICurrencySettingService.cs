namespace Fanex.Nap.CMS.WinLimitControl.Services.CurrencySetting
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Settings.Models;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.CurrencySetting;
    using Fanex.Nap.CMS.WinLimitControl.Models.CurrencySetting;

    public interface ICurrencySettingService
    {
        IList<WinLimitEntity> GetCurrencySettings(ICriteria criteria);

        IList<BaseSetting> GetExcludedCurrencies();

        IList<BaseSetting> GetExcludedGames();

        IList<BaseSetting> GetUnsetCurrencies(int gameId);

        IList<BaseSetting> GetUnsetGames(int currencyId);

        IList<Result> AddCurrency(BaseSetting currency, IList<AddCurrencyWinLimitCriteria> criterias);

        IList<Result> AddGame(BaseSetting game, IList<AddCurrencyWinLimitCriteria> criterias);

        IList<GreaterPlayerWinLimit> GetGreaterPlayerWinLimits(ICriteria criteria);

        Result UpdateGlobalWinLimit(UpdateCurrencyWinLimitCriteria criteria);
    }
}
