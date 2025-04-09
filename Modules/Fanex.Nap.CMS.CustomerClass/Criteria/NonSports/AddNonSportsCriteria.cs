namespace Fanex.Nap.CMS.CustomerClass.Criteria.NonSports
{
    internal class AddNonSportsCriteria : CustomerClassCommandBaseCriteria
    {
        public int PolicyID { get; set; }

        public string PolicyTitle { get; set; }

        public string CustomerClasses { get; set; }

        public string Sites { get; set; }

        public string Products { get; set; }

        public int MaxBet { get; set; }

        public bool Enable { get; set; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassNonSportsIns";
    }
}
