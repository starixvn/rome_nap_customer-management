namespace Fanex.Nap.CMS.CustomerClass.Criteria.NonSports
{
    internal class EnableNonSportsCriteria : CommandCriteriaBase
    {
        internal EnableNonSportsCriteria(int userId, string ip) : base(userId, ip)
        {
        }

        public int PolicyID { get; set; }

        public bool Enable { get; set; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassNonSportsEnable";

    }
}
