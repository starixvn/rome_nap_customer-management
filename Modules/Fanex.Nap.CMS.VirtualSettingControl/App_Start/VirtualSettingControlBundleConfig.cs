namespace Fanex.Nap.CMS.VirtualSettingControl
{
    using System.Web.Optimization;
    using Fanex.Nap.CMS.Infrastructure.Extensions;

    public static class VirtualSettingControlBundleConfig
    {
        public const string ESports_GlobalAutoSuspendThresholdScript = "~/bundles/virtual-rate-details/global-autosuspend-threshold";
        public const string ESports_GlobalAutoSuspendThresholdLogScript = "~/bundles/virtual-rate-details/global-autosuspend-threshold-log";

        public const string ESports_UploadVirtualRateStyle = "~/bundles/virtual-setting-control/upload-virtual-rate-style";
        public const string ESports_UploadVirtualRateScript = "~/bundles/virtual-setting-control/upload-virtual-rate-script";

        public const string ESports_ManualUpdateVRAccountStyle = "~/bundles/virtual-setting-control/manual-update-vr-account-style";
        public const string ESports_ManualUpdateVRAccountScript = "~/bundles/virtual-setting-control/manual-update-vr-account-script";

        public const string Basketball_VirtualRateDetailsScript = "~/bundles/virtual-setting-control/basketball-vr-details-script";
        public const string Basketball_VirtualRateGlobalSettingsScript = "~/bundles/virtual-setting-control/basketball-vr-globalsettings-script";
        public const string Basketball_VirtualRateLogsScript = "~/bundles/virtual-setting-control/basketball-vr-logs-script";
        public const string Basketball_AutoSuspendLogsScript = "~/bundles/virtual-setting-control/basketball-auto-suspend-logs-script";
        public const string Basketball_GlobalAutoSuspendThresholdLogsScript = "~/bundles/virtual-setting-control/basketball-global-autosuspend-threshold-logs-script";
        public const string Basketball_GlobalAutoSuspendThresholdScript = "~/bundles/virtual-setting-control/basketball-global-autosuspend-threshold-script";

        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterVirtualSettingMonitorBundles(bundles);
            RegisterCustomerDashboardBundles(bundles);
            RegisterVrAutoAdjustmentReportBundles(bundles);
            RegisterVirtualSettingMonitorAdvanceBundles(bundles);
            RegisterVirtualRateDetailsBundles(bundles);
            RegisterESports_GlobalAutoSuspendThreshold(bundles);
            RegisterESports_GlobalAutoSuspendThresholdLog(bundles);
            RegisterESports_UploadVirtualRate(bundles);
            RegisterESports_ManualUpdateVRAccount(bundles);
            RegisterBasketballVirtualRate(bundles);
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

        private static void RegisterVirtualRateDetailsBundles(BundleCollection bundles)
        {
            bundles.AddAndInclude(
                "~/bundles/virtual-rate-details/index-styles",
                "~/Areas/VirtualSettingControl/Views/VirtualRateDetails/Styles/index.less",
                "~/Areas/VirtualSettingControl/Views/VirtualRateDetails/Styles/member-report.less",
                "~/Areas/VirtualSettingControl/Views/VirtualRateDetails/Styles/auto-suspend-log.less");

            bundles.Add(
                new ScriptBundle("~/bundles/virtual-rate-details/index")
                .Include(
                "~/Areas/VirtualSettingControl/Views/VirtualRateDetails/Scripts/index.js",
                "~/Areas/VirtualSettingControl/Views/VirtualRateDetails/Scripts/member-vr.js"));

            bundles.Add(
              new ScriptBundle("~/bundles/soccer-virtual-rate-details/index")
              .Include(
              "~/Areas/VirtualSettingControl/Views/SoccerVirtualRateDetails/Scripts/index.js"
              ));

            bundles.Add(new ScriptBundle("~/bundles/virtual-rate-details/viewlog")
                .Include("~/Areas/VirtualSettingControl/Views/VirtualRateDetails/Scripts/viewlog.js"));

            bundles.Add(new ScriptBundle("~/bundles/soccer-virtual-rate-details/viewlog")
                .Include("~/Areas/VirtualSettingControl/Views/SoccerVirtualRateDetails/Scripts/viewlog.js"));

            bundles.Add(new ScriptBundle("~/bundles/virtual-rate-details/autosuspend-log")
                .Include("~/Areas/VirtualSettingControl/Views/VirtualRateDetails/Scripts/autosuspend-log.js"));
        }

        private static void RegisterESports_GlobalAutoSuspendThreshold(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(ESports_GlobalAutoSuspendThresholdScript)
                .Include("~/Areas/VirtualSettingControl/Views/VirtualRateDetails/Scripts/globalAutoSuspendThreshold.js"));
        }

        private static void RegisterESports_GlobalAutoSuspendThresholdLog(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(ESports_GlobalAutoSuspendThresholdLogScript)
                .Include("~/Areas/VirtualSettingControl/Views/VirtualRateDetails/Scripts/globalAutoSuspendThresholdLog.js"));
        }

        private static void RegisterESports_UploadVirtualRate(BundleCollection bundles)
        {
            bundles.AddAndInclude(
                ESports_UploadVirtualRateStyle,
                "~/Areas/VirtualSettingControl/Views/UploadVirtualRate/Styles/upload.less");

            bundles.Add(new ScriptBundle(ESports_UploadVirtualRateScript).Include(
                "~/Areas/VirtualSettingControl/Views/UploadVirtualRate/Scripts/UploadVirtualRate.js"));
        }

        private static void RegisterESports_ManualUpdateVRAccount(BundleCollection bundles)
        {
            bundles.AddAndInclude(
                ESports_ManualUpdateVRAccountStyle,
                "~/Areas/VirtualSettingControl/Views/ManualUpdateVRAccount/Styles/manualUpdate.less");

            bundles.Add(new ScriptBundle(ESports_ManualUpdateVRAccountScript)
                .Include("~/Areas/VirtualSettingControl/Views/ManualUpdateVRAccount/Scripts/manualUpdate.js"));
        }

        private static void RegisterBasketballVirtualRate(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(Basketball_VirtualRateDetailsScript)
                .Include(
                "~/Areas/VirtualSettingControl/Views/BasketballVirtualRate/Scripts/virtualRateGridEvents.js",
                "~/Areas/VirtualSettingControl/Views/BasketballVirtualRate/Scripts/virtualRateDetails.js"));

            bundles.Add(new ScriptBundle(Basketball_VirtualRateGlobalSettingsScript)
                .Include(
                "~/Areas/VirtualSettingControl/Views/BasketballVirtualRate/Scripts/virtualRateGridEvents.js",
                "~/Areas/VirtualSettingControl/Views/BasketballVirtualRate/Scripts/globalSettings.js"));

            bundles.Add(new ScriptBundle(Basketball_VirtualRateLogsScript)
                .Include(
                "~/Areas/VirtualSettingControl/Views/BasketballVirtualRate/Scripts/virtualRateLogGridEvents.js",
                "~/Areas/VirtualSettingControl/Views/BasketballVirtualRate/Scripts/virtualRateLogs.js"));

            bundles.Add(new ScriptBundle(Basketball_AutoSuspendLogsScript)
                .Include(
                "~/Areas/VirtualSettingControl/Views/BasketballVirtualRate/Scripts/virtualRateLogGridEvents.js",
                "~/Areas/VirtualSettingControl/Views/BasketballVirtualRate/Scripts/autoSuspendLogs.js"));

            bundles.Add(new ScriptBundle(Basketball_GlobalAutoSuspendThresholdLogsScript)
                .Include(
                "~/Areas/VirtualSettingControl/Views/BasketballVirtualRate/Scripts/virtualRateLogGridEvents.js",
                "~/Areas/VirtualSettingControl/Views/BasketballVirtualRate/Scripts/globalAutoSuspendThresholdLogs.js"));

            bundles.Add(new ScriptBundle(Basketball_GlobalAutoSuspendThresholdScript)
                .Include(
                "~/Areas/VirtualSettingControl/Views/BasketballVirtualRate/Scripts/virtualRateGridEvents.js",
                "~/Areas/VirtualSettingControl/Views/BasketballVirtualRate/Scripts/globalAutoSuspendThreshold.js"));
        }
    }
}
