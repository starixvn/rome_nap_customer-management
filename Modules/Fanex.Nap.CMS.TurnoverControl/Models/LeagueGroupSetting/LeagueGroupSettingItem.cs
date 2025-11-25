namespace Fanex.Nap.CMS.TurnoverControl.Models.LeagueGroupSetting
{
    using System;
    using System.Collections.Generic;
    using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;

    [Serializable]
    public class LeagueGroupSettingItem
    {
        public LeagueGroupSettingItem() { }
        public LeagueGroupSettingItem(
            int leagueGroupId,
            string leagueGroupName,
            short market,
            string marketName,
            IEnumerable<MaxTurnoverItem> maxTurnovers)
        {
            LeagueGroupId = leagueGroupId;
            LeagueGroupName = leagueGroupName;
            Market = market;
            MarketName = marketName;
            MaxTurnovers = maxTurnovers;
        }

        public int LeagueGroupId { get; }
        public string LeagueGroupName { get; }
        public short Market { get; }
        public string MarketName { get; }
        public IEnumerable<MaxTurnoverItem> MaxTurnovers { get; }
    }
}
