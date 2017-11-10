namespace Fanex.Nap.CMS.VirtualSettingControl
{
    using System.Web.Optimization;
    using Fanex.Nap.CMS.Infrastructure.Extensions;

    internal static class VirtualSettingControlBundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterVirtualSettingMonitorBundles(bundles);
            RegisterCustomerDashboardBundles(bundles);
            RegisterVrAutoAdjustmentReportBundles(bundles);
            RegisterVirtualSettingMonitorAdvanceBundles(bundles);
        }

        private static void RegisterVirtualSettingMonitorBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/virtualsettingcontrol/virtualsettingmonitor-scripts").Include(
                "~/Areas/VirtualSettingControl/Views/VirtualSettingMonitor/Scripts/ViewHistoryModal.js",
                "~/Areas/VirtualSettingControl/Views/VirtualSettingMonitor/Scripts/VirtualSettingMonitor.js",
                "~/Scripts/Libs/jQuery/jquery.cookie.min.js"));

            bundles.AddAndInclude(
                "~/bundles/virtualsettingcontrol/virtualsettingmonitor-styles",
                "~/Areas/VirtualSettingControl/Views/VirtualSettingMonitor/Styles/VirtualSettingMonitor.less");
        }

        private static void RegisterCustomerDashboardBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/scripts/dashboard").Include(
                "~/Scripts/Libs/jQuery/jquery.cookie.min.js",
                "~/Scripts/Libs/Highcharts/highcharts.js",
                "~/Scripts/Libs/Highcharts/highcharts-3d.js",
                "~/Areas/VirtualSettingControl/Views/CustomerDashboard/Script/dashboard-chart.js",
                "~/Areas/VirtualSettingControl/Views/CustomerDashboard/Script/dashboard.js"));

            bundles.AddAndInclude(
                "~/styles/dashboard",
                "~/Content/ToggleSwitch/toggle-switch.css",
                "~/Areas/VirtualSettingControl/Views/CustomerDashboard/Styles/dashboard.less");
        }

        private static void RegisterVrAutoAdjustmentReportBundles(BundleCollection bundles)
        {
            bundles.AddAndInclude(
                "~/bundles/VrAutoAdjustmentReport/css",
                "~/Areas/VirtualSettingControl/Views/VrAutoAdjustmentReport/Styles/vrAutoAdjustmentReport.css");

            bundles.Add(new ScriptBundle("~/bundles/VrAutoAdjustmentReport/js").Include(
                "~/Areas/VirtualSettingControl/Views/VrAutoAdjustmentReport/Scripts/VrAutoAdjustmentReport.js"));

            bundles.Add(new ScriptBundle("~/bundles/NewVrAutoAdjustmentReport/js").Include(
                "~/Areas/VirtualSettingControl/Views/NewVrAutoAdjustmentReport/Scripts/VrAutoAdjustmentReport.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Libs/MonthPicker/js").Include(
               "~/Scripts/Libs/MonthPicker/monPickr-0.0.3.js"));
        }

        private static void RegisterVirtualSettingMonitorAdvanceBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/VirtualSettingMonitorAdvance/js")
                .Include("~/Areas/VirtualSettingControl/Views/VirtualSettingMonitorAdvance/Scripts/virtualSettingMonitorAdvance.js"));

            bundles.AddAndInclude(
                "~/bundles/VirtualSettingMonitorAdvance/css",
                "~/Areas/VirtualSettingControl/Views/VirtualSettingMonitorAdvance/Styles/virtualSettingMonitorAdvance.less");

            bundles.AddAndInclude(
                "~/Content/MonthPicker/css",
                "~/Content/MonthPicker/monPickr-0.0.3-custom.less",
                "~/Content/MonthPicker/monPickr-0.0.3.less");
        }
    }
}