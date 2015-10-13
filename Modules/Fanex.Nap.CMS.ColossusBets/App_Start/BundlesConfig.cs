namespace Fanex.Nap.CMS.ColossusBets
{
    using System.Web.Optimization;

    public class BundlesConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/ColossusBets/PayoutStyles").Include(
                "~/Areas/ColossusBets/Views/Payout/Styles/payout.css"));

            bundles.Add(new ScriptBundle("~/bundles/ColossusBets/PayoutScripts").Include(
                "~/Areas/ColossusBets/Views/Payout/Scripts/payout.js",
                "~/Areas/ColossusBets/Views/Payout/Scripts/viewTicketDetail.js",
                "~/Areas/ColossusBets/Views/Payout/Scripts/updateTicketStatus.js",
                "~/Areas/ColossusBets/Views/Payout/Scripts/viewTicketNotes.js"));
        }
    }
}
