namespace Fanex.NAP.CMS.BetLimitTypeControl.Criteria
{
    using Fanex.Data.Repository;

    public class GetLeagueCriteria : CriteriaBase
    {
        public GetLeagueCriteria(
            string leagueGroupList,
            string leagueGroupListNotIn,
            string leagueName,
            string jsonSportList)
        {
            LeagueGroupList = leagueGroupList;
            LeagueGroupList_NotApply = leagueGroupListNotIn;
            LeagueName = leagueName;
            JsonSportList = jsonSportList;
        }

        public string LeagueGroupList { get; }

        public string LeagueGroupList_NotApply { get; }

        public string LeagueName { get; }

        public string JsonSportList { get; }

        public override string GetSettingKey()
            => "NAP_BLTypeControl_LeagueSelect";

        public override bool IsValid()
            => true;
    }
}
