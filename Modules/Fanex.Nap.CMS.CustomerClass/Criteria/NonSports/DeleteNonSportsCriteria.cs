namespace Fanex.Nap.CMS.CustomerClass.Criteria.NonSports
{
    internal class DeleteNonSportsCriteria : CustomerClassCommandBaseCriteria
    {
        public int PolicyID { get; set; }

        public string AllProducts { get; set; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassNonSportsDel";
    }
}
