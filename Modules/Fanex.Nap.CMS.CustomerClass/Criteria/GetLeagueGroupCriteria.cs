namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.NAP.Framework.Data;

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
    }
}
