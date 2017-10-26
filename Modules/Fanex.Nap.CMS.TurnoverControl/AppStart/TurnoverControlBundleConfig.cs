using System.Web.Optimization;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Transformers;

namespace Fanex.Nap.CMS.TurnoverControl.AppStart
{
    public static class TurnoverControlBundleConfig
    {
        public const string GlobalSettingScripts = "~/areas/turnovercontrol/globalsettingcripts";
        public const string GlobalSettingStyles = "~/areas/turnovercontrol/globalsettingstyles";

        public const string ImportGlobalSettingScripts = "~/areas/turnovercontrol/importglobalsettingcripts";
        public const string ImportGlobalSettingStyles = "~/areas/turnovercontrol/importglobalsettingstyles";

        public const string TurnoverMonitorScripts = "~/areas/turnovercontrol/turnovermonitor/turnovermonitorscripts";
        public const string TurnoverMonitorStyles = "~/areas/turnovercontrol/turnovermonitor/turnovermonitorstyles";

        public static void RegisterAll(BundleCollection bundles)
        {
            RegisterGlobalSettingBundles(bundles);

            RegisterTurnoverMonitorBundles(bundles);
            RegisterTurnoverMonitorCompanyTurnoverBundles(bundles);

            RegisterImportGlobalSettingBundles(bundles);
        }

        private static void RegisterGlobalSettingBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(GlobalSettingScripts).Include(
                "~/Scripts/Libs/Popup/dialogbox.js",
                "~/Areas/TurnoverControl/Views/GlobalSetting/Scripts/AddGlobalSetting.js",
                "~/Areas/TurnoverControl/Views/GlobalSetting/Scripts/DeleteGlobalSetting.js",
                "~/Areas/TurnoverControl/Views/GlobalSetting/Scripts/EditGlobalSetting.js",
                "~/Areas/TurnoverControl/Views/GlobalSetting/Scripts/GlobalSettingManager.js"));

            AddAndInclude(
                bundles,
                GlobalSettingStyles,
                "~/Areas/TurnoverControl/Views/GlobalSetting/Styles/GlobalSetting.less");
        }

        private static void RegisterImportGlobalSettingBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(ImportGlobalSettingScripts).Include(
                "~/Areas/TurnoverControl/Views/ImportGlobalSetting/Scripts/ValidSetting.js",
                "~/Areas/TurnoverControl/Views/ImportGlobalSetting/Scripts/ErrorSetting.js",
                "~/Areas/TurnoverControl/Views/ImportGlobalSetting/Scripts/ImportGlobalSetting.js"));

            AddAndInclude(
                bundles,
                ImportGlobalSettingStyles,
                "~/Areas/TurnoverControl/Views/ImportGlobalSetting/Styles/ImportGlobalSetting.less");
        }
        
        private static void RegisterTurnoverMonitorBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(TurnoverMonitorScripts).Include(
                "~/Scripts/Libs/Popup/dialogbox.js",
                "~/Areas/TurnoverControl/Views/TurnoverMonitor/Scripts/AutoRefresh.js",
                "~/Areas/TurnoverControl/Views/TurnoverMonitor/Scripts/HouseTurnoverPopup.js",
                "~/Areas/TurnoverControl/Views/TurnoverMonitor/Scripts/EditMaxTurnover.js",
                "~/Areas/TurnoverControl/Views/TurnoverMonitor/Scripts/EditMultipleMaxTurnover.js",
                "~/Areas/TurnoverControl/Views/TurnoverMonitor/Scripts/TurnoverMonitorManager.js"));

            AddAndInclude(
                bundles,
                TurnoverMonitorStyles,
                "~/Content/Themes/PopupCustomStyle.less",
                "~/Areas/TurnoverControl/Views/TurnoverMonitor/Styles/TurnoverMonitor.less");
        }

        private static void RegisterTurnoverMonitorCompanyTurnoverBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/areas/turnovercontrol/turnovermonitor/companyturnoverscripts").Include(
                "~/Areas/TurnoverControl/Views/TurnoverMonitor/CompanyTurnover/Scripts/CompanyTurnoverManager.js"));

            AddAndInclude(
                bundles,
                "~/areas/turnovercontrol/turnovermonitor/companyturnoverstyles",
                "~/Content/Themes/PopupCustomStyle.less",
                "~/Areas/TurnoverControl/Views/TurnoverMonitor/CompanyTurnover/Styles/CompanyTurnover.less");
        }

        private static void AddAndInclude(BundleCollection bundles, string bundleVirtualPath, params string[] virtualPaths)
        {
            bundles.Add(new Bundle(bundleVirtualPath)
            {
                Transforms = { new StyleTransformer(), new CssMinify() },
                Orderer = new NullOrderer()
            }.Include(virtualPaths));
        }
    }
}
