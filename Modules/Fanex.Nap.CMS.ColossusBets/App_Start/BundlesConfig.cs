namespace Fanex.Nap.CMS.ColossusBets
{
    using System.Web.Optimization;

    public class BundlesConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/ColossusBets/PaymentStyles").Include(
                "~/Areas/ColossusBets/Views/Payment/Styles/payment.css"));

            bundles.Add(new ScriptBundle("~/bundles/ColossusBets/PaymentScripts").Include(
                "~/Areas/ColossusBets/Views/Payment/Scripts/payment.js",
                "~/Areas/ColossusBets/Views/Payment/Scripts/viewTicketDetail.js",
                "~/Areas/ColossusBets/Views/Payment/Scripts/updateTicketStatus.js",
                "~/Areas/ColossusBets/Views/Payment/Scripts/viewTicketNotes.js"));
        }
    }
}
