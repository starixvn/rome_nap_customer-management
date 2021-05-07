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
        public const string BetLimitTypeControlScriptPath = "~/bundles/bet-limit-type-control-scripts";
        public const string BetLimitTypeControlStylePath = "~/bundles/bet-limit-type-control-styles";
        public const string BetLimitControlScriptPath = "~/bundles/bet-limit-control-scripts";
        public const string BetLimitControlStylePath = "~/bundles/bet-limit-control-styles";
        public const string DefaultMemberBetLimitScriptPath = "~/bundles/default-member-bet-limit-scripts";
        public const string DefaultMemberBetLimitStylePath = "~/bundles/default-member-bet-limit-styles";

        public const string LimitNewDownlineScriptPath = "~/bundles/limit-new-downline-scripts";
        public const string LimitNewDownlineStylePath = "~/bundles/limit-new-downline-styles";

        public const string WhitelistBigCreditMembersScriptPath = "~/bundles/whitelist-big-credit-members-scripts";
        public const string WhitelistBigCreditMembersStylePath = "~/bundles/whitelist-big-credit-members-styles";

        private const string FanexMultiSelectScriptPath = "~/Scripts/Libs/Fanex.Multiselect/fanex.multiSelect.js";
        private const string FanexMultiSelectStylePath = "~/Scripts/Libs/Fanex.Multiselect/fanex.multiSelect.css";

        public const string VirtualRateEngineScriptPath = "~/bundles/customers/virtual-rate-engine-script";
        public const string VirtualRateEngineStylePath = "~/bundles/customers/virtual-rate-engine-style";
        public const string VREngineCustomerCategoryScriptPath = "~/bundles/customers/vr-engine-customer-category-script";
        public const string VREngineCustomerCategoryStylePath = "~/bundles/customers/vr-engine-customer-category-style";

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles
                .RegisterProblemAccountBundles()
                .RegisterBetListBundles()
                .RegisterPossessStatisticsBundles()
                .RegisterProblemAccountCreditSiteBundles()
                .RegisterProblemAccountLicenseeSiteBundles()
                .RegisterManageLicenseeAcknowledgementBundles()
                .RegisterBetLimitTypeControlBundles()
                .RegisterBetLimitControlBundles()
                .RegisterDefaultMemberBetLimitBundles()
                .RegisterLimitNewDownlineBundles()
                .RegisterWhitelistBigCreditMembersBundles()
                .RegisterVirtualRateEngine();

            RegisterManageRobots2(bundles);
        }

        private static BundleCollection RegisterProblemAccountBundles(this BundleCollection bundles)
        {
            bundles.AddAndInclude(
                ProblemAccountStyles,
                FanexMultiSelectStylePath,
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
                FanexMultiSelectStylePath,
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
                "~/Areas/Customers/Scripts/problemAccount-unmark.js",
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

        private static BundleCollection RegisterBetLimitTypeControlBundles(this BundleCollection bundles)
        {
            bundles.AddAndInclude(
                BetLimitTypeControlStylePath,
                FanexMultiSelectStylePath,
                "~/Scripts/Libs/jquery-timepicker/jquery.timepicker.css",
                "~/Content/Pages/BetLimitTypeControl/index.less");

            bundles.Add(new ScriptBundle(BetLimitTypeControlScriptPath).Include(
                FanexMultiSelectScriptPath,
                "~/Scripts/Libs/jquery-timepicker/jquery.timepicker.js",
                "~/Content/Pages/BetLimitTypeControl/index.js"));

            return bundles;
        }

        private static BundleCollection RegisterBetLimitControlBundles(this BundleCollection bundles)
        {
            bundles.AddAndInclude(
                BetLimitControlStylePath,
                FanexMultiSelectStylePath,
                "~/Content/Pages/BetLimitControl/index.less");

            bundles.Add(new ScriptBundle(BetLimitControlScriptPath).Include(
                "~/Scripts/Libs/InputFormatter/number-commar-seperator.js",
                FanexMultiSelectScriptPath,
                "~/Content/Pages/BetLimitControl/index.js"));

            return bundles;
        }

        private static BundleCollection RegisterDefaultMemberBetLimitBundles(this BundleCollection bundles)
        {
            bundles.AddAndInclude(
                DefaultMemberBetLimitStylePath,
                FanexMultiSelectStylePath,
                "~/Content/Pages/DefaultMemberBetLimit/index.less");

            bundles.Add(new ScriptBundle(DefaultMemberBetLimitScriptPath).Include(
                "~/Scripts/Libs/InputFormatter/number-commar-seperator.js",
                FanexMultiSelectScriptPath,
                "~/Content/Pages/DefaultMemberBetLimit/index.js"));

            return bundles;
        }

        private static BundleCollection RegisterLimitNewDownlineBundles(this BundleCollection bundles)
        {
            bundles.AddAndInclude(
                LimitNewDownlineStylePath,
                FanexMultiSelectStylePath,
                "~/Areas/Customers/Styles/limitNewDownline.less");

            bundles.Add(new ScriptBundle(LimitNewDownlineScriptPath).Include(
                FanexMultiSelectScriptPath,
                "~/Areas/Customers/Scripts/LimitNewDownline/addNewOrRemoveLimit.js",
                "~/Areas/Customers/Scripts/LimitNewDownline/limitNewDownline.js"));

            return bundles;
        }

        private static BundleCollection RegisterWhitelistBigCreditMembersBundles(this BundleCollection bundles)
        {
            bundles.AddAndInclude(
               WhitelistBigCreditMembersStylePath,
               "~/Areas/Customers/Styles/whitelistBigCreditMembers.less");

            bundles.Add(new ScriptBundle(WhitelistBigCreditMembersScriptPath).Include(
                "~/Areas/Customers/Scripts/WhitelistBigCreditMembers/whitelistBigCreditMembers.js"));

            return bundles;
        }

        private static BundleCollection RegisterVirtualRateEngine(this BundleCollection bundles)
        {
            bundles.AddAndInclude(
                VirtualRateEngineStylePath,
                "~/Scripts/Libs/Fanex.Multiselect/jquery.multiselect.css",
                "~/Areas/Customers/Styles/virtualRateEngine.less");

            bundles.Add(new ScriptBundle(VirtualRateEngineScriptPath).Include(
                "~/Scripts/Libs/Fanex.Multiselect/jquery.multiselect.js",
                "~/Areas/Customers/Scripts/VirtualRateEngine/VREngineHelper.js",
                "~/Areas/Customers/Scripts/VirtualRateEngine/siteTypeCriteria.js",
                "~/Areas/Customers/Scripts/VirtualRateEngine/customerClassCriteria.js",
                "~/Areas/Customers/Scripts/VirtualRateEngine/currentVRCriteria.js",
                "~/Areas/Customers/Scripts/VirtualRateEngine/currentVRStatusCriteria.js",
                "~/Areas/Customers/Scripts/VirtualRateEngine/sportCriteria.js",
                "~/Areas/Customers/Scripts/VirtualRateEngine/betTypeCriteria.js",
                "~/Areas/Customers/Scripts/VirtualRateEngine/eventCriteria.js",
                "~/Areas/Customers/Scripts/VirtualRateEngine/leagueGroupCriteria.js",
                "~/Areas/Customers/Scripts/VirtualRateEngine/betfromCriteria.js",
                "~/Areas/Customers/Scripts/VirtualRateEngine/stakeCriteria.js",
                "~/Areas/Customers/Scripts/VirtualRateEngine/ticketDoWCriteria.js",
                "~/Areas/Customers/Scripts/VirtualRateEngine/ticketTimeCriteria.js",
                "~/Areas/Customers/Scripts/VirtualRateEngine/VREngineHelperRuleList.js",
                "~/Areas/Customers/Scripts/VirtualRateEngine/virtualRateEngine.js"));

            bundles.AddAndInclude(
                VREngineCustomerCategoryStylePath,
                "~/Areas/Customers/Styles/customerCategory.less");

            bundles.Add(new ScriptBundle(VREngineCustomerCategoryScriptPath).Include(
                "~/Areas/Customers/Scripts/VirtualRateEngine/customerCategory.js"));

            return bundles;
        }
    }
}