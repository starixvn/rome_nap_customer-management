namespace Fanex.Nap.CMS.ColossusBets
{
    using System.Web.Optimization;
    using Fanex.Nap.CMS.Infrastructure.Extensions;

    internal static class BundlesConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterBonusTicketsBundles(bundles);
            RegisterBonusTicketDetailBundles(bundles);
        }

        private static void RegisterBonusTicketsBundles(BundleCollection bundles)
        {
            bundles.AddAndInclude(
                "~/bundles/ColossusBets/BonusTicketStyles",
                "~/Areas/ColossusBets/Views/BonusTicket/Styles/bonusTicket.css");

            bundles.Add(new ScriptBundle("~/bundles/ColossusBets/BonusTicketScripts").Include(
                "~/Areas/ColossusBets/Views/BonusTicket/Scripts/bonusTicket.js"));
        }

        private static void RegisterBonusTicketDetailBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/ColossusBets/BonusTicketDetailScripts").Include(
                "~/Areas/ColossusBets/Views/BonusTicket/Scripts/bonusTicketDetail.js"));
        }
    }
}
