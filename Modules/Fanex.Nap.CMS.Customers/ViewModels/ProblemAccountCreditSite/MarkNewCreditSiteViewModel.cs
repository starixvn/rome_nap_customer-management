namespace Fanex.Nap.CMS.Customers.ViewModels.ProblemAccountCreditSite
{
    using ProblemAccount;

    public class MarkNewCreditSiteViewModel : MarkNewViewModel
    {
        public byte PreventiveAction { get; set; }

        public override bool IsInvalid()
            => base.IsInvalid() || PreventiveAction <= 0;
    }
}
