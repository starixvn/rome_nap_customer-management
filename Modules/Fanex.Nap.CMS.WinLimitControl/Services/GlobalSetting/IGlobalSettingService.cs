namespace Fanex.Nap.CMS.WinLimitControl.Services.GlobalSetting
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Settings.Models;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.GlobalSetting;
    using Fanex.Nap.CMS.WinLimitControl.Models.GlobalSetting;

    public interface IGlobalSettingService
    {
        IList<GlobalWinLimit> GetGlobalWinLimits(ICriteria criteria);

        IList<BaseSetting> GetExcludedCurrencies();

        IList<BaseSetting> GetExcludedGames();

        IList<BaseSetting> GetUnsetCurrencies(int gameId);

        IList<BaseSetting> GetUnsetGames(int currencyId);

        IList<Result> AddCurrency(BaseSetting currency, IList<AddGlobalWinLimitCriteria> criterias);

        IList<Result> AddGame(BaseSetting game, IList<AddGlobalWinLimitCriteria> criterias);

        IList<GreaterPlayerWinLimit> GetGreaterPlayerWinLimits(ICriteria criteria);

        Result UpdateGlobalWinLimit(UpdateGlobalWinLimitCriteria criteria);
    }
}
