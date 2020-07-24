namespace Fanex.Nap.CMS.Customers.ViewModels.ProblemAccountCreditSite
{
    using ProblemAccount;

    public class MarkNewCreditSiteViewModel : MarkNewViewModel
    {
        public byte PreventiveAction { get; set; }

        public string VoidTime { get; set; }

        public decimal LimitAmount { get; set; }

        public bool UpdateTime { get; set; }

        public bool UpdateLimitAmount { get; set; }

        public override bool IsInvalid()
            => base.IsInvalid() || PreventiveAction <= 0;
    }
}
