// -----------------------------------------------------------------------
// <copyright file="NotificationSettingBundleConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Nap.CMS.NotificationSetting.AppStart
{
    using System.Web.Optimization;
    using BundleTransformer.Core.Orderers;
    using BundleTransformer.Core.Transformers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class NotificationSettingBundleConfig
    {
        public static void RegisterAll(BundleCollection bundles)
        {
            RegisterTurnoverControlBundles(bundles);
        }

        /// <summary>
        /// Registers the turn over bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterTurnoverControlBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/areas/notificationsetting/turnovercontrol/turnovercontrolscripts").Include(
                "~/Scripts/Libs/Popup/dialogbox.js",
                "~/Areas/NotificationSetting/Views/TurnoverControl/Scripts/TokenInput/jquery.tokeninput.js",
                "~/Areas/NotificationSetting/Views/TurnoverControl/Scripts/MailingList.js"));

            AddAndInclude(
                bundles,
                "~/areas/notificationsetting/turnovercontrol/turnovercontrolstyles",
                "~/Areas/NotificationSetting/Views/TurnoverControl/Scripts/TokenInput/token-input-facebook.css",
                "~/Areas/NotificationSetting/Views/TurnoverControl/Styles/MailingList.less");
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