namespace Fanex.Nap.CMS.Customers.AppStart
{
    using System.Web.Optimization;
    using Infrastructure.Extensions;

    public static class CustomersBundlesConfig
    {
        public const string ProblemAccountStyles = "~/bundles/customers/problem-account-styles";
        public const string ProblemAccountScript = "~/bundles/customers/problem-account-scripts";
        public const string AbnormalBetTicketScript = "~/bundles/customers/betlist-abnormalbetticket-scripts";
        public const string PossessStatisticsStyles = "~/bundles/customers/possess-statistics-styles";
        public const string PossessStatisticsScript = "~/bundles/customers/possess-statistics-scripts";
        public const string ProblemAccountCreditSiteScript = "~/bundles/customers/problem-account-creditsite-scripts";
        public const string ProblemAccountLicenseeSiteScript = "~/bundles/customers/problem-account-licenseesite-scripts";
        public const string ManageLicenseeAcknowledgementScript = "~/bundles/customers/manage-licensee-acknowledgement-scripts";

        private const string FanexMultiSelectScriptPath = "~/Scripts/Libs/Fanex.Multiselect/fanex.multiSelect.js";

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles
                .RegisterProblemAccountBundles()
                .RegisterBetListBundles()
                .RegisterPossessStatisticsBundles()
                .RegisterProblemAccountCreditSiteBundles()
                .RegisterProblemAccountLicenseeSiteBundles()
                .RegisterManageLicenseeAcknowledgementBundles();

            RegisterManageRobots2(bundles);
        }

        private static BundleCollection RegisterProblemAccountBundles(this BundleCollection bundles)
        {
            bundles.AddAndInclude(
                ProblemAccountStyles,
                "~/Scripts/Libs/Fanex.Multiselect/fanex.multiSelect.css",
                "~/Areas/Customers/Styles/problemAccount.less");

            bundles.Add(new ScriptBundle(ProblemAccountScript).Include(
                FanexMultiSelectScriptPath,
                "~/Areas/Customers/Scripts/problemAccount-search.js",
                "~/Areas/Customers/Scripts/problemAccount-marknew.js",
                "~/Areas/Customers/Scripts/problemAccount-unmark.js",
                "~/Areas/Customers/Scripts/problemAccount-reduceBL.js",
                "~/Areas/Customers/Scripts/problemAccount-confirmAction.js",
                "~/Areas/Customers/Scripts/problemAccount-informingStatus.js",
                "~/Areas/Customers/Scripts/problemAccount.js"));

            return bundles;
        }

        private static BundleCollection RegisterBetListBundles(this BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(AbnormalBetTicketScript).Include(
                "~/Areas/Customers/Scripts/betlist-abnormalbetticket.js"));

            return bundles;
        }

        private static void RegisterManageRobots2(BundleCollection bundles)
        {
            bundles.AddAndInclude(
                "~/bundles/manage-robots-2/index-styles",
                "~/Scripts/Libs/Fanex.Multiselect/fanex.multiSelect.css",
                "~/Areas/Customers/Views/ManageRobots2/Styles/index.less");

            bundles.Add(new ScriptBundle("~/bundles/manage-robots-2/index-scripts")
                .Include(
                "~/Scripts/Libs/Fanex.Multiselect/fanex.multiSelect.js",
                "~/Areas/Customers/Views/ManageRobots2/Scripts/index.js"));
        }

        private static BundleCollection RegisterPossessStatisticsBundles(this BundleCollection bundles)
        {
            bundles.AddAndInclude(
                PossessStatisticsStyles,
                "~/Scripts/Libs/Fanex.Multiselect/fanex.multiSelect.css",
                "~/Areas/Customers/Styles/possessStatistics.less");

            bundles.Add(new ScriptBundle(PossessStatisticsScript).Include(
                "~/Scripts/Libs/Fanex.Multiselect/fanex.multiSelect.js",
                "~/Areas/Customers/Scripts/PossessStatistics/data-grid.js",
                "~/Areas/Customers/Scripts/PossessStatistics/filter.js",
                "~/Areas/Customers/Scripts/PossessStatistics/update-action.js",
                "~/Areas/Customers/Scripts/PossessStatistics/index.js"));

            return bundles;
        }

        private static BundleCollection RegisterProblemAccountCreditSiteBundles(this BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(ProblemAccountCreditSiteScript).Include(
                FanexMultiSelectScriptPath,
                "~/Areas/Customers/Scripts/ProblemAccountCreditSite/problemAccountCreditSite-search.js",
                "~/Areas/Customers/Scripts/ProblemAccountCreditSite/problemAccountCreditSite-marknew.js",
                "~/Areas/Customers/Scripts/ProblemAccountCreditSite/problemAccountCreditSite-update.js",
                "~/Areas/Customers/Scripts/problemAccount-unmark.js",
                "~/Areas/Customers/Scripts/ProblemAccountCreditSite/problemAccountCreditSite-reduceBL.js",
                "~/Areas/Customers/Scripts/problemAccount-confirmAction.js",
                "~/Areas/Customers/Scripts/ProblemAccountCreditSite/problemAccountCreditSite.js"));

            return bundles;
        }

        private static BundleCollection RegisterProblemAccountLicenseeSiteBundles(this BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(ProblemAccountLicenseeSiteScript).Include(
                FanexMultiSelectScriptPath,
                "~/Areas/Customers/Scripts/ProblemAccountLicenseeSite/problemAccountLicenseeSite-search.js",
                "~/Areas/Customers/Scripts/ProblemAccountLicenseeSite/problemAccountLicenseeSite-marknew.js",
                "~/Areas/Customers/Scripts/ProblemAccountLicenseeSite/problemAccountLicenseeSite-update.js",
                "~/Areas/Customers/Scripts/ProblemAccountLicenseeSite/problemAccountLicenseeSite-reduceBL.js",
                "~/Areas/Customers/Scripts/problemAccount-confirmAction.js",
                "~/Areas/Customers/Scripts/ProblemAccountLicenseeSite/problemAccountLicenseeSite-informingStatus.js",
                "~/Areas/Customers/Scripts/ProblemAccountLicenseeSite/problemAccountLicenseeSite.js"));

            return bundles;
        }

        private static BundleCollection RegisterManageLicenseeAcknowledgementBundles(this BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(ManageLicenseeAcknowledgementScript).Include(
                "~/Areas/Customers/Scripts/ManageLicenseeAcknowledgement/manageLicensee.js"));

            return bundles;
        }

    }
}
