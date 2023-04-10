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

        private const string FanexMultiSelectStyle = "~/Scripts/Libs/Fanex.Multiselect/fanex.multiSelect.css";
        private const string FanexMultiSelectCustomStyle = "~/Content/MultipleSelect/fanex.multiSelect-custom.less";
        private const string FanexMultiSelectScript = "~/Scripts/Libs/Fanex.Multiselect/fanex.multiSelect.js";

        public static void RegisterAll(BundleCollection bundles)
            => bundles
            .RegisterCustomerClassSettingBundles()
            .RegisterRuleBundles()
            .RegisterPolicyBundles()
            .RegisterSiteBundles();

        private static BundleCollection RegisterCustomerClassSettingBundles(this BundleCollection bundles)
        {
            bundles.AddAndInclude(
                CustomerClassSettingStyles,
                "~/Areas/CustomerClass/Styles/customerClassSetting.less");

            bundles.Add(new ScriptBundle(CustomerClassSettingScripts).Include(
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
    }
}
