using System.Collections.Generic;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.TurnoverControl.Models;
using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;

namespace Fanex.Nap.CMS.TurnoverControl.Services.GlobalSetting
{
    public interface IGlobalSettingService
    {
        IEnumerable<IdNameModel> GetCurrencies();

        GlobalSettingModel GetList(
            string username,
            short sportType,
            int exchangeId,
            string sortField,
            string sortDirection,
            int page,
            int size,
            bool cache);

        IEnumerable<IdNameModel> GetProfiles(short sportType);

        Result Add(string username, MaxTurnoverItem[] maxTurnovers, bool isGroupHeadline, short sportType, int userId, string ip);

        Result Edit(UpdateMaxTurnoverParam param, int userId, string ip);

        Result Delete(int custId, bool isGroupHeadline, short sportType, int userId, string ip);

        Result Lock(LockSettingParam param, int userId, string ip);
    }
}
