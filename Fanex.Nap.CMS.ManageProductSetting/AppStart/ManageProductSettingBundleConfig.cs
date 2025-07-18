namespace Fanex.Nap.CMS.ManageProductSetting.AppStart
{
    using System.Web.Optimization;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    public static class ManageProductSettingBundleConfig
    {
        public const string CompanyBetLimitSettingScripts = "~/areas/manageproductsetting/companybetlimitsettingscripts";
        public const string CompanyBetLimitSettingStyles = "~/areas/manageproductsetting/companybetlimitsettingstyles";
        
        public static void RegisterAll(BundleCollection bundles)
        {
            RegisterCompanyBetLimitSettingBundles(bundles);
        }
        private static void RegisterCompanyBetLimitSettingBundles(BundleCollection bundles)
        {
            bundles
                .Add(new ScriptBundle(CompanyBetLimitSettingScripts)
                    .Include(
                        "~/Scripts/Libs/Fanex.Multiselect/jquery.multiselect.js",
                        "~/Scripts/Libs/Popup/dialogbox.js",
                        "~/Scripts/Tabs.js",
                        "~/Areas/ManageProductSetting/Scripts/CompanyBetLimitSetting/index.js",
                        "~/Areas/ManageProductSetting/Scripts/CompanyBetLimitSetting/importExcel.js",
                        "~/Areas/ManageProductSetting/Scripts/CompanyBetLimitSetting/currencyGroup.js"
                    )
                );
            bundles.AddAndInclude(
                CompanyBetLimitSettingStyles,
                "~/Scripts/Libs/Fanex.Multiselect/jquery.multiselect.css",
                "~/Areas/ManageProductSetting/Styles/CompanyBetLimitSetting/index.less");

        }

    }
}