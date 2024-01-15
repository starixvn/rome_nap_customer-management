namespace Fanex.Nap.CMS.CustomerClass.Criteria.NonSports
{
    internal class EditNonSportsCriteria : AddNonSportsCriteria
    {
        internal EditNonSportsCriteria(int userId, string ip) : base(userId, ip)
        {
        }

        public string AllProducts { get; set; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassNonSportsUpd";
    }
}
