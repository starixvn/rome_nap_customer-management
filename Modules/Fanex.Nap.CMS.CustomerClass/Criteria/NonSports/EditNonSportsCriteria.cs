namespace Fanex.Nap.CMS.CustomerClass.Criteria.NonSports
{
    internal class EditNonSportsCriteria : AddNonSportsCriteria
    {
        public string AllProducts { get; set; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassNonSportsUpd";
    }
}
