namespace Fanex.Nap.CMS.VirtualSettingControl
{
    using System.Web.Optimization;
    using Fanex.Nap.CMS.Infrastructure.Extensions;

    internal static class VirtualSettingControlBundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterVirtualSettingonitorBundles(bundles);
        }

        private static void RegisterVirtualSettingonitorBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/virtualsettingcontrol/virtualsettingmonitor-scripts").Include(
                "~/Areas/VirtualSettingControl/Views/VirtualSettingMonitor/Scripts/ViewHistoryModal.js",
                "~/Areas/VirtualSettingControl/Views/VirtualSettingMonitor/Scripts/VirtualSettingMonitor.js",
                "~/Scripts/Libs/jQuery/jquery.cookie.min.js"));

            bundles.AddAndInclude(
                "~/bundles/virtualsettingcontrol/virtualsettingmonitor-styles",
                "~/Areas/VirtualSettingControl/Views/VirtualSettingMonitor/Styles/VirtualSettingMonitor.less");
        }
    }
}
