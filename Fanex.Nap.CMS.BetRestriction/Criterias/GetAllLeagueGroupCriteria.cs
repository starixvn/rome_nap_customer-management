namespace Fanex.Nap.CMS.BetRestriction.Criterias
{
    using Fanex.Data.Repository;

    public class GetAllLeagueGroupCriteria : CriteriaBase
    {
        public GetAllLeagueGroupCriteria(int sportType)
        {
            SportType = sportType;
        }

        public int SportType { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_BetRestriction_LeagueGroupSel";
        }

        public override bool IsValid()
            => true;
    }
}
