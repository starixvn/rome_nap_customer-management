namespace Fanex.Nap.CMS.TurnoverControl.Services.LeagueGroupSetting
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.TurnoverControl.Models.LeagueGroupSetting;
    using Fanex.Nap.CMS.TurnoverControl.ViewModels.LeagueGroup;

    public interface ITurnoverLeagueGroupService
    {
        TurnoverLeagueGroupViewModel GetLeagueGroupViewModel(short sportType);
        IEnumerable<LeagueGroupSettingItem> GetLeagueGroupSettings(short sportType, int leagueGroupId, short market);
    }
}
