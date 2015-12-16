namespace Fanex.Nap.CMS.WinLimitControl
{
    using System.Web.Optimization;
    using Fanex.Nap.CMS.Infrastructure.Extensions;

    internal static class WinLimitControlBundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterGlobalSettingBundles(bundles);
            RegisterPlayerSettingBundles(bundles);
            RegisterWinLimitMonitorBundles(bundles);
        }

        private static void RegisterGlobalSettingBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/winlimitcontrol/currencysettingscripts").Include(
                "~/Areas/WinLimitControl/Views/CurrencySetting/Scripts/EditWinLimitModal.js",
                "~/Areas/WinLimitControl/Views/CurrencySetting/Scripts/CurrencySetting.js"));

            bundles.AddAndInclude(
                "~/bundles/winlimitcontrol/currencysettingstyles",
                "~/Areas/WinLimitControl/Views/CurrencySetting/Styles/currencySetting.css");
        }

        private static void RegisterPlayerSettingBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/winlimitcontrol/playersettingscripts").Include(
                "~/Areas/WinLimitControl/Views/PlayerSetting/Scripts/AddPlayerModal.js",
                "~/Areas/WinLimitControl/Views/PlayerSetting/Scripts/PlayerSetting.js"));

            bundles.AddAndInclude(
                "~/bundles/winlimitcontrol/playersettingstyles",
                "~/Areas/WinLimitControl/Views/PlayerSetting/Styles/PlayerSetting.css");
        }

        private static void RegisterWinLimitMonitorBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/winlimitcontrol/winlimitmonitor-scripts").Include(
                "~/Areas/WinLimitControl/Views/WinLimitMonitor/Scripts/ViewHistoryModal.js",
                "~/Areas/WinLimitControl/Views/WinLimitMonitor/Scripts/WinLimitMonitor.js",
                "~/Scripts/Libs/jQuery/jquery.cookie.min.js"));

            bundles.AddAndInclude(
                "~/bundles/winlimitcontrol/winlimitmonitor-styles",
                "~/Areas/WinLimitControl/Views/WinLimitMonitor/Styles/WinLimitMonitor.less");
        }
    }
}
