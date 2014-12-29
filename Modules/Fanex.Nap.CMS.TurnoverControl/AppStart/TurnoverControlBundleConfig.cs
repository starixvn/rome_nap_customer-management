// --------------------------------------------------------------------------
// <copyright file="TurnoverControlBundleConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.Nap.CMS.TurnoverControl.AppStart
{
    using System.Web.Optimization;
    using BundleTransformer.Core.Orderers;
    using BundleTransformer.Core.Transformers;

    /// <summary>
    /// Turnover Control Bundle Config.
    /// </summary>
    public static class TurnoverControlBundleConfig
    {
        public static void RegisterAll(BundleCollection bundles)
        {
            RegisterGlobalSettingBundles(bundles);
            RegisterAddGlobalSettingBundles(bundles);

            RegisterTurnoverMonitorBundles(bundles);
            RegisterTurnoverMonitorCompanyTurnoverBundles(bundles);
        }

        /// <summary>
        /// Registers the global setting bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterGlobalSettingBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/areas/turnovercontrol/globalsettingscripts").Include(
                "~/Scripts/Libs/Popup/dialogbox.js",
                "~/Areas/TurnoverControl/Views/GlobalSetting/Scripts/GlobalSettingManager.js"));

            AddAndInclude(
                bundles,
                "~/areas/turnovercontrol/globalsettingstyles",
                "~/Areas/TurnoverControl/Views/GlobalSetting/Styles/GlobalSetting.less");
        }

        /// <summary>
        /// Registers the add global setting bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterAddGlobalSettingBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/areas/turnovercontrol/globalsetting/addsettingscripts").Include(
                "~/Areas/TurnoverControl/Views/GlobalSetting/AddSetting/Scripts/AddSettingManager.js"));

            AddAndInclude(
                bundles,
                "~/areas/turnovercontrol/globalsetting/addsettingstyles",
                "~/Content/Themes/PopupCustomStyle.less",
                "~/Areas/TurnoverControl/Views/GlobalSetting/AddSetting/Styles/AddSetting.less");
        }

        /// <summary>
        /// Registers the turnover monitor bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterTurnoverMonitorBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/areas/turnovercontrol/turnovermonitor/turnovermonitorscripts").Include(
                "~/Scripts/Libs/Popup/dialogbox.js",
                "~/Areas/TurnoverControl/Views/TurnoverMonitor/Scripts/TurnoverMonitorManager.js"));

            AddAndInclude(
                bundles,
                "~/areas/turnovercontrol/turnovermonitor/turnovermonitorstyles",
                "~/Content/Themes/PopupCustomStyle.less",
                "~/Areas/TurnoverControl/Views/TurnoverMonitor/Styles/TurnoverMonitor.less");
        }

        /// <summary>
        /// Registers the view company turnover bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
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
