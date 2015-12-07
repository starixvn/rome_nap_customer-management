namespace Fanex.Nap.CMS.PayoutManagement
{
    using System.Web.Optimization;
    using Fanex.Nap.CMS.Infrastructure.Extensions;

    internal static class BundlesConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterPayoutListBundles(bundles);
            RegisterPayoutDetailsBundles(bundles);
        }

        private static void RegisterPayoutListBundles(BundleCollection bundles)
        {
            bundles.AddAndInclude(
                "~/bundles/PayoutManagement/PayoutListStyles",
                "~/Scripts/Libs/Fanex.Multiselect/fanex.multiSelect.css",
                "~/Areas/PayoutManagement/Views/PayoutList/Styles/payoutList.css");

            bundles.Add(new ScriptBundle("~/bundles/PayoutManagement/PayoutListScripts").Include(
                "~/Scripts/Libs/Fanex.Multiselect/fanex.multiSelect.js",
                "~/Areas/PayoutManagement/Views/PayoutList/Scripts/payoutList.js"));
        }

        private static void RegisterPayoutDetailsBundles(BundleCollection bundles)
        {
            bundles.AddAndInclude(
                "~/bundles/PayoutManagement/PayoutDetailStyles",
                "~/Areas/PayoutManagement/Views/PayoutList/Styles/payoutDetail.css");

            bundles.Add(new ScriptBundle("~/bundles/PayoutManagement/PayoutDetailScripts").Include(
                "~/Areas/PayoutManagement/Views/PayoutList/Scripts/payoutDetail.js"));
        }
    }
}
