namespace Fanex.Nap.CMS.Customers.Criteria.VirtualRateEngine
{
    using Fanex.Data.Repository;

    public class GetVREngineLeagueCriteria : CriteriaBase
    {
        public GetVREngineLeagueCriteria(
            string leagueGroupList,
            string leagueGroupListNotIn,
            string leagueName)
        {
            LeagueGroupList = leagueGroupList;
            LeagueGroupList_NotApply = leagueGroupListNotIn;
            LeagueName = leagueName;
        }

        public string LeagueGroupList { get; }

        public string LeagueGroupList_NotApply { get; }

        public string LeagueName { get; }

        public override string GetSettingKey()
            => "NAP_VREngine_League_Sel";
    }
}
