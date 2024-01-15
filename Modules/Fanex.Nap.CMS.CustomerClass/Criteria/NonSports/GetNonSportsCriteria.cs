namespace Fanex.Nap.CMS.CustomerClass.Criteria.NonSports
{
    internal class GetNonSportsCriteria : QueryCriteriaBase
    {
        public string CustomerClasses { get; set; }

        public string Sites { get; set; }

        public string Products { get; set; }

        public string AllProducts { get; set; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassNonSportsSel";
    }
}
