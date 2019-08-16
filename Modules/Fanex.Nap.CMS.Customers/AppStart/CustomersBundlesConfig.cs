namespace Fanex.Nap.CMS.Customers.AppStart
{
    using System.Web.Optimization;
    using Infrastructure.Extensions;

    public static class CustomersBundlesConfig
    {
        public const string ProblemAccountStyles = "~/bundles/customers/problem-account-styles";
        public const string ProblemAccountScript = "~/bundles/customers/problem-account-scripts";

        public const string AbnormalBetTicketScript = "~/bundles/customers/betlist-abnormalbetticket-scripts";

        public static void RegisterBundles(BundleCollection bundles)
            => bundles
                .RegisterProblemAccountBundles()
                .RegisterBetListBundles();

        private static BundleCollection RegisterProblemAccountBundles(this BundleCollection bundles)
        {
            bundles.AddAndInclude(
                ProblemAccountStyles,
                "~/Scripts/Libs/Fanex.Multiselect/fanex.multiSelect.css",
                "~/Areas/Customers/Styles/problemAccount.less");

            bundles.Add(new ScriptBundle(ProblemAccountScript).Include(
                "~/Scripts/Libs/Fanex.Multiselect/fanex.multiSelect.js",
                "~/Areas/Customers/Scripts/problemAccount-search.js",
                "~/Areas/Customers/Scripts/problemAccount-marknew.js",
                "~/Areas/Customers/Scripts/problemAccount-unmark.js",
                "~/Areas/Customers/Scripts/problemAccount-reduceBL.js",
                "~/Areas/Customers/Scripts/problemAccount-confirmAction.js",
                "~/Areas/Customers/Scripts/problemAccount.js"));

            return bundles;
        }

        private static BundleCollection RegisterBetListBundles(this BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(AbnormalBetTicketScript).Include(
                "~/Areas/Customers/Scripts/betlist-abnormalbetticket.js"));

            return bundles;
        }
    }
}
