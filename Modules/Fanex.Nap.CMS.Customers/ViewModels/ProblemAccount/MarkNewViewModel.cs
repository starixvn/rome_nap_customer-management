namespace Fanex.Nap.CMS.Customers.ViewModels.ProblemAccount
{
    using System.Web.Mvc;

    public class MarkNewViewModel : ActionViewModel
    {
        public byte Reason { get; set; }

        [AllowHtml]
        public string Comment { get; set; }

        public override bool IsInvalid()
            => base.IsInvalid() || Reason <= 0;
    }
}
