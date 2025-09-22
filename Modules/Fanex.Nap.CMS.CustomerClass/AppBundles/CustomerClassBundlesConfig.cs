namespace Fanex.Nap.CMS.CustomerClass.AppBundles
{
    using System.Web.Optimization;
    using Fanex.Nap.CMS.Infrastructure.Extensions;

    public static class CustomerClassBundlesConfig
    {
        public const string CustomerClassSettingStyles = "~/bundles/customerclass/customer-class-setting-styles";
        public const string CustomerClassSettingScripts = "~/bundles/customerclass/customer-class-setting-scripts";

        public const string RuleStyles = "~/bundles/customerclass/rule-styles";
        public const string RuleScripts = "~/bundles/customerclass/rule-scripts";

        public const string PolicyStyles = "~/bundles/customerclass/policy-styles";
        public const string PolicyScripts = "~/bundles/customerclass/policy-scripts";

        public const string SiteStyles = "~/bundles/customerclass/site-styles";
        public const string SiteScripts = "~/bundles/customerclass/site-scripts";

        public const string SpreadStyles = "~/bundles/customerclass/spread-styles";
        public const string SpreadScripts = "~/bundles/customerclass/spread-scripts";

        public const string PeakHourStyles = "~/bundles/customerclass/peakhour-styles";
        public const string PeakHourScripts = "~/bundles/customerclass/peakhour-scripts";

        public const string OfferingStyles = "~/bundles/customerclass/offering-styles";
        public const string OfferingScripts = "~/bundles/customerclass/offering-scripts";

        public const string CCNonsportsStyles = "~/bundles/customerclass/nonsports-styles";
        public const string CCNonsportsScripts = "~/bundles/customerclass/nonsports-scripts";

        private const string FanexMultiSelectStyle = "~/Scripts/Libs/Fanex.Multiselect/fanex.multiSelect.css";
        private const string FanexMultiSelectCustomStyle = "~/Content/MultipleSelect/fanex.multiSelect-custom.less";
        private const string FanexMultiSelectScript = "~/Scripts/Libs/Fanex.Multiselect/fanex.multiSelect.js";

        public static void RegisterAll(BundleCollection bundles)
            => bundles
            .RegisterCustomerClassSettingBundles()
            .RegisterRuleBundles()
            .RegisterPolicyBundles()
            .RegisterSiteBundles()
            .RegisterSpreadBundles()
            .RegisterPeakHourBundles()
            .RegisterOfferingBundles()
            .RegisterNonsports();

        private static BundleCollection RegisterCustomerClassSettingBundles(this BundleCollection bundles)
        {
            bundles.AddAndInclude(
                CustomerClassSettingStyles,
                FanexMultiSelectStyle,
                FanexMultiSelectCustomStyle,
                "~/Areas/CustomerClass/Styles/customerClassSetting.less");

            bundles.Add(new ScriptBundle(CustomerClassSettingScripts).Include(
                FanexMultiSelectScript,
                "~/Areas/CustomerClass/Scripts/customerClassHelper.js",
                "~/Areas/CustomerClass/Scripts/ruleManagementHelper.js",
                "~/Areas/CustomerClass/Scripts/customerClassSetting.js"));

            return bundles;
        }

        private static BundleCollection RegisterRuleBundles(this BundleCollection bundles)
        {
            bundles.AddAndInclude(
                RuleStyles,
                FanexMultiSelectStyle,
                FanexMultiSelectCustomStyle,
                "~/Areas/CustomerClass/Styles/ruleManagement.less");

            bundles.Add(new ScriptBundle(RuleScripts).Include(
                FanexMultiSelectScript,
                "~/Areas/CustomerClass/Scripts/customerClassHelper.js",
                "~/Areas/CustomerClass/Scripts/ruleManagementHelper.js",
                "~/Areas/CustomerClass/Scripts/ruleManagementAddOrEdit.js",
                "~/Areas/CustomerClass/Scripts/ruleManagement.js"));

            return bundles;
        }

        private static BundleCollection RegisterPolicyBundles(this BundleCollection bundles)
        {
            bundles.AddAndInclude(
                PolicyStyles,
                "~/Areas/CustomerClass/Styles/policyManagement.less");

            bundles.Add(new ScriptBundle(PolicyScripts).Include(
                "~/Areas/CustomerClass/Scripts/customerClassHelper.js",
                "~/Areas/CustomerClass/Scripts/policyManagementHelper.js",
                "~/Areas/CustomerClass/Scripts/policyManagementAddOrEdit.js",
                "~/Areas/CustomerClass/Scripts/policyManagement.js"));

            return bundles;
        }

        private static BundleCollection RegisterSiteBundles(this BundleCollection bundles)
        {
            bundles.AddAndInclude(
                SiteStyles,
                FanexMultiSelectStyle,
                FanexMultiSelectCustomStyle,
                "~/Areas/CustomerClass/Styles/siteManagement.less");

            bundles.Add(new ScriptBundle(SiteScripts).Include(
                FanexMultiSelectScript,
                "~/Areas/CustomerClass/Scripts/customerClassHelper.js",
                "~/Areas/CustomerClass/Scripts/siteManagementAssignSiteSetting.js",
                "~/Areas/CustomerClass/Scripts/siteManagementUpload.js",
                "~/Areas/CustomerClass/Scripts/siteManagement.js"));

            return bundles;
        }

        private static BundleCollection RegisterSpreadBundles(this BundleCollection bundles)
        {
            bundles.AddAndInclude(
                SpreadStyles,
                FanexMultiSelectStyle,
                FanexMultiSelectCustomStyle,
                "~/Areas/CustomerClass/Styles/spreadManagement.less");

            bundles.Add(new ScriptBundle(SpreadScripts).Include(
                FanexMultiSelectScript,
                "~/Scripts/Libs/autoNumeric/autoNumeric-4.6.0.min.js",
                "~/Areas/CustomerClass/Scripts/customerClassHelper.js",
                "~/Areas/CustomerClass/Scripts/spreadManagementHelper.js",
                "~/Areas/CustomerClass/Scripts/editingGridHelper.js",
                "~/Areas/CustomerClass/Scripts/spreadManagementUpload.js",
                "~/Areas/CustomerClass/Scripts/spreadManagement.js"));

            return bundles;
        }
        private static BundleCollection RegisterPeakHourBundles(this BundleCollection bundles)
        {
            bundles.AddAndInclude(
                PeakHourStyles,
                FanexMultiSelectStyle,
                FanexMultiSelectCustomStyle,
                "~/Scripts/Libs/jquery-timepicker/jquery.timepicker.css",
                "~/Areas/CustomerClass/Styles/spreadManagement.less",
                "~/Areas/CustomerClass/Styles/peakHourManagement.less");

            bundles.Add(new ScriptBundle(PeakHourScripts).Include(
                FanexMultiSelectScript,
                "~/Scripts/Libs/autoNumeric/autoNumeric-4.6.0.min.js",
                "~/Areas/CustomerClass/Scripts/customerClassHelper.js",
                "~/Areas/CustomerClass/Scripts/spreadManagementHelper.js",
                "~/Areas/CustomerClass/Scripts/editingGridHelper.js",
                "~/Scripts/Libs/jquery-timepicker/jquery.timepicker.js",
                "~/Areas/CustomerClass/Scripts/peakHourManagement.js",
                "~/Areas/CustomerClass/Scripts/adjustPeakHour.js"));

            return bundles;
        }

        private static BundleCollection RegisterOfferingBundles(this BundleCollection bundles)
        {
            bundles.AddAndInclude(
                OfferingStyles,
                FanexMultiSelectStyle,
                FanexMultiSelectCustomStyle,
                "~/Areas/CustomerClass/Styles/editingGrid.less",
                "~/Areas/CustomerClass/Styles/offering.less");

            bundles.Add(new ScriptBundle(OfferingScripts).Include(
                FanexMultiSelectScript,
                "~/Scripts/Libs/autoNumeric/autoNumeric-4.6.0.min.js",
                "~/Areas/CustomerClass/Scripts/customerClassHelper.js",
                "~/Areas/CustomerClass/Scripts/editingGridHelper.js",
                "~/Areas/CustomerClass/Scripts/offering.js"));

            return bundles;
        }

        private static BundleCollection RegisterNonsports(this BundleCollection bundles)
        {
            bundles.AddAndInclude(
                CCNonsportsStyles,
                FanexMultiSelectStyle,
                FanexMultiSelectCustomStyle,
                "~/Areas/CustomerClass/Styles/editingGrid.less");

            bundles.Add(new ScriptBundle(CCNonsportsScripts).Include(
                FanexMultiSelectScript,
                "~/Scripts/Libs/autoNumeric/autoNumeric-4.6.0.min.js",
                "~/Areas/CustomerClass/Scripts/customerClassHelper.js",
                "~/Areas/CustomerClass/Scripts/editingGridHelper.js",
                "~/Areas/CustomerClass/Scripts/nonsports.js"));

            return bundles;
        }
    }
}
