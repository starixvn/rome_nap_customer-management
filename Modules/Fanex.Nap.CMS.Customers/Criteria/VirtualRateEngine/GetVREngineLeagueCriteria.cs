using Fanex.Data.Repository;

namespace Fanex.Nap.CMS.Customers.Criteria.VirtualRateEngine
{
    public class GetVREngineLeagueCriteria : CriteriaBase
    {
        public GetVREngineLeagueCriteria(string leagueGroupList, string leagueName)
        {
            LeagueGroupList = leagueGroupList;
            LeagueName = leagueName;
        }

        public string LeagueGroupList { get; }

        public string LeagueName { get; }

        public override string GetSettingKey()
            => "NAP_VREngine_League_Sel";
    }
}
