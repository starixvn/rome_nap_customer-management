namespace Fanex.Nap.CMS.TurnoverControl.Criteria.LeagueGroupSetting
{
    using Fanex.Data.Repository;

    public class GetLeagueGroupSettingCriteria : CriteriaBase
    {
        public GetLeagueGroupSettingCriteria(short sportType, int leagueGroupId, short market)
        {
            SportType = sportType;
            LeagueGroupId = leagueGroupId;
            Market = market;
        }

        public short SportType { get; }
        public int LeagueGroupId { get; }
        public short Market { get; set; }

        public override string GetSettingKey()
            => "";

        public override bool IsValid()
            => true;
    }
}
