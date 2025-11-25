namespace Fanex.Nap.CMS.TurnoverControl.Services.LeagueGroupSetting
{
    using System;
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.TurnoverControl.Criteria.LeagueGroupSetting;
    using Fanex.Nap.CMS.TurnoverControl.Models;
    using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;
    using Fanex.Nap.CMS.TurnoverControl.Models.LeagueGroupSetting;
    using Fanex.Nap.CMS.TurnoverControl.ViewModels.LeagueGroup;

    public class TurnoverLeagueGroupService : TurnoverBaseService, ITurnoverLeagueGroupService
    {
        public TurnoverLeagueGroupViewModel GetLeagueGroupViewModel(short sportType)
        {
            var profiles = GetTOProfiles(sportType);
            var leagueGroup = GetLeagueGroups(sportType);
            var market = GetMarkets();
            var settings = GetRandomLeagueGroupSettings(leagueGroup, market, profiles);
            var viewModel = new TurnoverLeagueGroupViewModel(
                leagueGroup,
                market,
                profiles,
                settings
            );

            return viewModel;
        }

        public IEnumerable<LeagueGroupSettingItem> GetLeagueGroupSettings(short sportType, int leagueGroupId, short market)
        {
            var criteria = new GetLeagueGroupSettingCriteria(sportType, leagueGroupId, market);
            var settings = DynamicRepository.Fetch<LeagueGroupSettingItem>(criteria);

            return settings;
        }

        public IEnumerable<LeagueGroupSettingItem> GetRandomLeagueGroupSettings(IEnumerable<LeagueGroup> leagueGroups, IEnumerable<IdNameModel> markets, IEnumerable<IdNameModel> profiles)
        {
            var random = new Random(500000);
            var settings = new List<LeagueGroupSettingItem>();
            foreach (var leagueGroup in leagueGroups)
            {
                foreach (var market in markets)
                {
                    var profileSettings = new List<MaxTurnoverItem>();
                    foreach (var profile in profiles)
                    {
                        profileSettings.Add(new MaxTurnoverItem(profile.Id, random.Next()));
                    }

                    settings.Add(new LeagueGroupSettingItem(
                        leagueGroup.Id,
                        leagueGroup.Name,
                        (short)market.Id,
                        market.Name,
                        profileSettings));
                }
            }

            return settings;
        }
    }
}
