namespace Fanex.Nap.CMS.TurnoverControl.Criteria
{
    using Fanex.Data.Repository;

    internal class GetLeagueGroupCriteria : CriteriaBase
    {
        public GetLeagueGroupCriteria()
        {
        }

        public GetLeagueGroupCriteria(int sportType)
        {
            SportType = sportType;
        }

        public int SportType { get; set; }

        public override string GetSettingKey()
            => "Nap_LM_CustomerClassLeagueGroupSel";

        public override bool IsValid()
            => true;
    }
}
