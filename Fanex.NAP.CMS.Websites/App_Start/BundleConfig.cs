// --------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.3">Created</change>
// </history>

namespace Fanex.NAP.CMS.Websites
{
    using System.Web.Optimization;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.NotificationSetting.AppStart;
    using Fanex.Nap.CMS.TurnoverControl.AppStart;

    /// <summary>
    /// Bundle Configuration.
    /// </summary>
    public static class BundleConfig
    {
        /// <summary>
        /// Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundle collections.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterLoginBundles(bundles);
            RegisterCommonBundles(bundles);
            RegisterCustomerBundles(bundles);
            RegisterTurnoverControlBundles(bundles);
            RegisterLossControlBundles(bundles);

            TurnoverControlBundleConfig.RegisterAll(bundles);
            NotificationSettingBundleConfig.RegisterAll(bundles);
        }

        /// <summary>
        /// Registers the common bundles.
        /// </summary>
        /// <param name="bundles">The bundle collection.</param>
        public static void RegisterLoginBundles(BundleCollection bundles)
        {
            bundles.AddAndInclude(
                "~/styles/login",
                "~/Content/Sites/Login/login.less");

            bundles.Add(new ScriptBundle("~/scripts/loginJS").Include(
                "~/Scripts/Libs/Crypto/CryptoJS.js",
                "~/Scripts/Sites/Login.js"));
        }

        /// <summary>
        /// Registers the common bundles.
        /// </summary>
        /// <param name="bundles">The bundle collection.</param>
        public static void RegisterCommonBundles(BundleCollection bundles)
        {
            bundles.AddAndInclude(
                "~/styles/common",
                "~/Content/Themes/jquery/jquery-ui-1.9.1.custom.min.css",
                "~/Scripts/Libs/Fanex.Grid/Fanex.Grid.css",
                "~/Scripts/Libs/Fanex.Grid/Fanex.Pagination.css",
                "~/Scripts/Libs/DatePicker/jquery.nexDatePicker.css",
                "~/Scripts/Libs/Tipsy/tipsy.css",
                "~/Scripts/Libs/msgbox/msgbox-0.0.1.css",
                "~/Content/Extensions/jquery.combobox-ext.less",
                "~/Content/Extensions/jquery.extensions.less",
                "~/Content/Themes/reset.css",
                "~/Content/Themes/style.less");

            bundles.Add(new ScriptBundle("~/scripts/common")
                .Include("~/Scripts/Libs/jQuery/jquery-1.8.2.min.js")
                .Include("~/Scripts/Libs/jQuery/jquery-migrate-1.2.1.js")
                .Include("~/Scripts/Libs/jQuery/jquery-ui-1.8.24.min.js")
                .Include("~/Scripts/Libs/jQuery/jquery.mask.min.js")
                .Include("~/Scripts/Libs/jQuery/jquery.blockUI.1.33.min.js")
                .Include("~/Scripts/Libs/Fanex.Grid/Fanex.Grid.js")
                .Include("~/Scripts/Libs/Fanex.Grid/Fanex.Pagination.js")
                .Include("~/Scripts/Libs/Fanex.Grid/moment.js")
                .Include("~/Scripts/Libs/Fanex.Grid/numeral.js")
                .Include("~/Scripts/Libs/DatePicker/jquery.nexDatePicker.js")
                .Include("~/Scripts/Libs/msgbox/msgbox-0.0.1.js")
                .Include("~/Scripts/Libs/Tipsy/jquery.tipsy.js")
                .Include("~/Scripts/Libs/others/Namespace.js")
                .Include("~/Scripts/Extensions/nativeObjectExtensions.js")
                .Include("~/Scripts/Extensions/jquery.combobox-ext.js")
                .Include("~/Scripts/Extensions/jquery.extensions.js")
                .Include("~/Scripts/Validator.js")
                .Include("~/Scripts/KeyboardManager.js")
                .Include("~/Scripts/CMS.js")
                .Include("~/Scripts/CMSConstants.js")
                .Include("~/Scripts/Layout.js"));

            bundles.Add(new ScriptBundle("~/scripts/crypto").Include(
                "~/Scripts/Libs/Crypto/CryptoJS.js"));
        }

        /// <summary>
        /// Registers the customer bundles.
        /// </summary>
        /// <param name="bundles">The bundle collection.</param>
        public static void RegisterCustomerBundles(BundleCollection bundles)
        {
            // Open/Close Member.
            bundles.AddAndInclude(
                "~/styles/openCloseMember",
                "~/Content/Sites/openCloseMember.less");

            bundles.Add(new ScriptBundle("~/scripts/openCloseMember").Include(
                "~/Scripts/Sites/OpenCloseMember.js"));

            // Configure Customer Access.
            bundles.AddAndInclude(
                "~/styles/configureCustomerAccess",
                "~/Content/Sites/configureCustomerAccess.less");

            bundles.Add(new ScriptBundle("~/scripts/configureCustomerAccess").Include(
                "~/Scripts/Sites/ConfigureCustomerAccess.js"));

            // Manage Robots.
            bundles.AddAndInclude(
                "~/styles/manageRobots",
                "~/Content/Sites/manageRobots.less");

            bundles.Add(new ScriptBundle("~/scripts/manageRobots").Include(
                "~/Scripts/Sites/ManageRobots.js"));

            // Manage Smart Punters.
            bundles.AddAndInclude(
                "~/styles/manageSmartPunters",
                "~/Content/Sites/manageSmartPunters.less");

            bundles.Add(new ScriptBundle("~/scripts/manageSmartPunters").Include(
                "~/Scripts/Sites/ManageSmartPunters/PageManager.js",
                "~/Scripts/Sites/ManageSmartPunters/ManageSmartPunters.js",
                "~/Scripts/Sites/ManageSmartPunters/ControlSmartPuntersLog.js"));
        }

        /// <summary>
        /// Registers the turnover control bundles.
        /// </summary>
        /// <param name="bundles">The bundle collection.</param>
        public static void RegisterTurnoverControlBundles(BundleCollection bundles)
        {
            // Configure TO By Event.
            bundles.AddAndInclude(
                "~/styles/configureTOByEvent",
                "~/Content/Sites/configureTOByEvent.less");

            bundles.Add(new ScriptBundle("~/scripts/configureTOByEvent").Include(
                "~/Scripts/Sites/ConfigureTOByEvent/SportPage.js",
                "~/Scripts/Sites/ConfigureTOByEvent/LeaguePage.js",
                "~/Scripts/Sites/ConfigureTOByEvent/MatchPage.js",
                "~/Scripts/Sites/ConfigureTOByEvent/PageManager.js"));

            // Configure TO By SMA.
            bundles.AddAndInclude(
                "~/styles/configureTOBySMA",
                "~/Content/Sites/configureTOBySMA.less");

            bundles.Add(new ScriptBundle("~/scripts/configureTOBySMA").Include(
                "~/Scripts/Sites/ConfigureTOBySMA.js"));

            // Configure SMA TO By Event.
            bundles.AddAndInclude(
                "~/styles/configureSMATOByEvent",
                "~/Content/Sites/configureSMATOByEvent.less");

            bundles.Add(new ScriptBundle("~/scripts/configureSMATOByEvent").Include(
                "~/Scripts/Sites/ConfigureSMATOByEvent.js"));

            // Turnover Usage.
            bundles.AddAndInclude(
                "~/styles/turnoverUsage",
                "~/Content/Sites/turnoverUsage.less");

            bundles.Add(new ScriptBundle("~/scripts/turnoverUsage").Include(
                "~/Scripts/Sites/TurnoverUsage.js"));

            // Turnover Monitor.
            bundles.AddAndInclude(
                "~/styles/turnoverMonitor",
                "~/Content/Sites/turnoverMonitor.less");

            bundles.Add(new ScriptBundle("~/scripts/turnoverMonitor").Include(
                "~/Scripts/Sites/TurnoverMonitor.js"));
        }

        /// <summary>
        /// Registers the loss control bundles.
        /// </summary>
        /// <param name="bundles">The bundle collection.</param>
        public static void RegisterLossControlBundles(BundleCollection bundles)
        {
            // Loss Monitor.
            bundles.AddAndInclude(
                "~/styles/lossMonitor",
                "~/Content/Sites/lossMonitor.less");

            bundles.Add(new ScriptBundle("~/scripts/lossMonitor").Include(
                "~/Scripts/Libs/Popup/dialogbox.js",
                "~/Scripts/Sites/LossMonitor.js"));
        }
    }
}