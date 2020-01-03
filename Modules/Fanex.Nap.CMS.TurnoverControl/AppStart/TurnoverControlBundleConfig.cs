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

        public const string TurnoverMonitorScripts = "~/areas/turnovercontrol/turnovermonitorscripts";
        public const string TurnoverMonitorStyles = "~/areas/turnovercontrol/turnovermonitorstyles";

        public static void RegisterAll(BundleCollection bundles)
        {
            RegisterGlobalSettingBundles(bundles);
            RegisterImportGlobalSettingBundles(bundles);

            RegisterTurnoverMonitorBundles(bundles);
        }

        private static void RegisterGlobalSettingBundles(BundleCollection bundles)
        {
            bundles
                .Add(new ScriptBundle(GlobalSettingScripts)
                .Include(
                    "~/Scripts/Libs/Popup/dialogbox.js",
                    "~/Areas/TurnoverControl/Scripts/GlobalSetting/AddGlobalSetting.js",
                    "~/Areas/TurnoverControl/Scripts/GlobalSetting/DeleteGlobalSetting.js",
                    "~/Areas/TurnoverControl/Scripts/GlobalSetting/EditGlobalSetting.js",
                    "~/Areas/TurnoverControl/Scripts/GlobalSetting/GlobalSettingManager.js"));

            AddStyle(
                bundles,
                GlobalSettingStyles,
                "~/Areas/TurnoverControl/Styles/GlobalSetting.less");
        }

        private static void RegisterImportGlobalSettingBundles(BundleCollection bundles)
        {
            bundles
                .Add(new ScriptBundle(ImportGlobalSettingScripts)
                .Include(
                    "~/Areas/TurnoverControl/Scripts/ImportGlobalSetting/ValidSetting.js",
                    "~/Areas/TurnoverControl/Scripts/ImportGlobalSetting/ErrorSetting.js",
                    "~/Areas/TurnoverControl/Scripts/ImportGlobalSetting/ImportGlobalSetting.js"));

            AddStyle(
                bundles,
                ImportGlobalSettingStyles,
                "~/Areas/TurnoverControl/Styles/ImportGlobalSetting.less");
        }
        
        private static void RegisterTurnoverMonitorBundles(BundleCollection bundles)
        {
            bundles
                .Add(new ScriptBundle(TurnoverMonitorScripts)
                .Include(
                    "~/Scripts/Libs/Popup/dialogbox.js",
                    "~/Areas/TurnoverControl/Scripts/TurnoverMonitor/AutoRefresh.js",
                    "~/Areas/TurnoverControl/Scripts/TurnoverMonitor/HouseTurnoverPopup.js",
                    "~/Areas/TurnoverControl/Scripts/TurnoverMonitor/EditMaxTurnover.js",
                    "~/Areas/TurnoverControl/Scripts/TurnoverMonitor/EditMultipleMaxTurnover.js",
                    "~/Areas/TurnoverControl/Scripts/TurnoverMonitor/TurnoverMonitorManager.js"));

            AddStyle(
                bundles,
                TurnoverMonitorStyles,
                "~/Content/Themes/PopupCustomStyle.less",
                "~/Areas/TurnoverControl/Styles/TurnoverMonitor.less");
        }

        private static void AddStyle(BundleCollection bundles, string bundleVirtualPath, params string[] virtualPaths)
            => bundles.Add(new Bundle(bundleVirtualPath)
            {
                Transforms = { new StyleTransformer(), new CssMinify() },
                Orderer = new NullOrderer()
            }.Include(virtualPaths));
    }
}
