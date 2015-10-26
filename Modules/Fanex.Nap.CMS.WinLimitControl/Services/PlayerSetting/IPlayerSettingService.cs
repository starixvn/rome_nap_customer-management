namespace Fanex.Nap.CMS.WinLimitControl.Services.PlayerSetting
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Settings.Models;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.PlayerSetting;
    using Fanex.Nap.CMS.WinLimitControl.Models.PlayerSetting;

    public interface IPlayerSettingService
    {
        IList<dynamic> CustomerAutoSuggestion(ICriteria criteria);

        IList<PlayerWinLimit> GetPlayerSettings(GetPlayerWinLimitCriteria criteria);

        IList<UndefinedPlayerWinLimit> GetUndefinedGameByPlayer(GetUndefinedGameByPlayerCriteria criteria);

        Result AddPlayerWinLimit(IList<AddPlayerWinLimitCriteria> criterias);

        Result UpdatePlayerWinLimit(UpdatePlayerWinLimitCriteria criteria);

        Result DeletePlayerWinLimit(ICriteria criterias);
    }
}
