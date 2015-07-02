namespace Fanex.Nap.CMS.WinLimitControl.Repositories.PlayerWinLimitManagement
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.WinLimitControl.Models.PlayerWinLimitManagement;

    public interface IPlayerSettingRepository
    {
        IList<PlayerWinLimitEntity> GetPlayerWinLimits(ICriteria criteria);

        AddOrUpdatePlayerWinLimit GetUnsetGameByPlayer(ICriteria criteria);

        Result DeletePlayerWinLimit(ICriteria criteria);
    }
}
