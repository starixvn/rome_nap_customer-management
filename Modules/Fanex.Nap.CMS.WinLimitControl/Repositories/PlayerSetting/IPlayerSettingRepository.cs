namespace Fanex.Nap.CMS.WinLimitControl.Repositories.PlayerSetting
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Settings.Models;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.PlayerSetting;
    using Fanex.Nap.CMS.WinLimitControl.Models.PlayerSetting;

    public interface IPlayerSettingRepository
    {
        IList<dynamic> CustomerAutoSuggestion(ICriteria criteria);

        IList<PlayerWinLimitEntity> GetPlayerSettings(GetPlayerWinLimitCriteria criteria);

        IList<UndefinedPlayerWinLimit> GetUndefinedGameByPlayer(GetUndefinedGameByPlayerCriteria criteria);

        Result AddPlayerWinLimit(AddPlayerWinLimitCriteria criteria);

        Result UpdatePlayerWinLimit(UpdatePlayerWinLimitCriteria criteria);

        Result DeletePlayerWinLimit(ICriteria criteria);
    }
}
