namespace Fanex.Nap.CMS.Customers.Criteria.VirtualRateEngine
{
    using Fanex.Data.Repository;

    public class GetVREngineLeagueCriteria : CriteriaBase
    {
        public GetVREngineLeagueCriteria(
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
            => "NAP_VREngine_League_Sel";
    }
}
