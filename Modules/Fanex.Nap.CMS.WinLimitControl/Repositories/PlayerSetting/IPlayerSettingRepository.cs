namespace Fanex.Nap.CMS.WinLimitControl.Repositories.PlayerSetting
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.PlayerSetting;
    using Fanex.Nap.CMS.WinLimitControl.Models.PlayerSetting;

    public interface IPlayerSettingRepository
    {
        IList<PlayerWinLimitEntity> GetPlayerWinLimits(GetPlayerWinLimitCriteria criteria);

        AddOrUpdatePlayerWinLimit GetUnsetGameByPlayer(ICriteria criteria);

        IList<GameWinLimit> GetPlayerWinLimitsForUpdate(GetPlayerWinLimitForUpdateCriteria criteria);

        Result AddPlayerWinLimit(AddPlayerWinLimitCriteria criteria);

        Result UpdatePlayerWinLimit(UpdatePlayerWinLimitCriteria criteria);

        Result DeletePlayerWinLimit(ICriteria criteria);
    }
}
