namespace Fanex.Nap.CMS.TurnoverControl.ViewModels.LeagueGroup
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.TurnoverControl.Models;
    using Fanex.Nap.CMS.TurnoverControl.Models.LeagueGroupSetting;

    public class TurnoverLeagueGroupViewModel
    {
        public TurnoverLeagueGroupViewModel(
            IEnumerable<LeagueGroup> leagueGroups,
            IEnumerable<IdNameModel> markets,
            IEnumerable<IdNameModel> profiles,
            IEnumerable<LeagueGroupSettingItem> leagueGroupSettings)
        {
            Markets = markets ?? new List<IdNameModel>();
            LeagueGroups = leagueGroups ?? new List<LeagueGroup>();
            Profiles = profiles ?? new List<IdNameModel>();
            LeagueGroupSettings = leagueGroupSettings ?? new List<LeagueGroupSettingItem>();
        }
        public IEnumerable<LeagueGroup> LeagueGroups { get; }
        public IEnumerable<IdNameModel> Markets { get; }
        public IEnumerable<IdNameModel> Profiles { get; }
        public IEnumerable<LeagueGroupSettingItem> LeagueGroupSettings { get; }
    }
}
