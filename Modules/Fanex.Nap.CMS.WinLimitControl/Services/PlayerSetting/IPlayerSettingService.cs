namespace Fanex.Nap.CMS.WinLimitControl.Services.PlayerSetting
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.PlayerSetting;
    using Fanex.Nap.CMS.WinLimitControl.Models.PlayerSetting;

    public interface IPlayerSettingService
    {
        IList<PlayerWinLimit> GetPlayerWinLimits(GetPlayerWinLimitCriteria criteria);

        AddOrUpdatePlayerWinLimit GetUnsetGameByPlayer(GetUnsetGameByPlayerCriteria criteria);

        IList<GameWinLimit> GetPlayerWinLimitsForUpdate(GetPlayerWinLimitForUpdateCriteria criteria);

        Result AddPlayerWinLimit(IList<AddPlayerWinLimitCriteria> criterias);

        Result UpdatePlayerWinLimit(IList<UpdatePlayerWinLimitCriteria> criterias);

        Result DeletePlayerWinLimit(ICriteria criterias);
    }
}
