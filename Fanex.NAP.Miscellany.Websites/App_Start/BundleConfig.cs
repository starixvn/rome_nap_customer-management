// --------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.3">Created</change>
// </history>

namespace Fanex.NAP.Miscellany.Websites
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Optimization;
    using BundleTransformer.Core.Orderers;
    using BundleTransformer.Core.Transformers;

    /// <summary>
    /// Bundle Configuration.
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundle collections.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterLoginBundles(bundles);
            RegisterCommonBundles(bundles);
            RegisterOpenCloseMemberBundles(bundles);
        }

        /// <summary>
        /// Registers the common bundles.
        /// </summary>
        /// <param name="bundles">The bundle collection.</param>
        public static void RegisterLoginBundles(BundleCollection bundles)
        {
            bundles.AddAndInclude(
                "~/styles/login",
                "~/Areas/UserLogin/themes/login.less");

            bundles.Add(new ScriptBundle("~/scripts/loginJS").Include(
                "~/Scripts/Libs/jQuery/jquery-1.8.2.js"));
        }

        /// <summary>
        /// Registers the common bundles.
        /// </summary>
        /// <param name="bundles">The bundle collection.</param>
        public static void RegisterCommonBundles(BundleCollection bundles)
        {
            bundles.AddAndInclude(
                "~/styles/common",
                "~/Content/Themes/jquery/jquery-ui-1.9.1.custom.css",
                "~/Content/Themes/reset.css",
                "~/Content/Themes/style.less",
                "~/Scripts/Libs/Fanex.Grid/Fanex.Grid.css",
                "~/Scripts/Libs/Fanex.Grid/Fanex.Pagination.css",
                "~/Scripts/Libs/DatePicker/jquery.nexDatePicker.css",
                "~/Scripts/Libs/Tipsy/tipsy.css",
                "~/Scripts/Libs/msgbox/msgbox-0.0.1.css");

            bundles.Add(new ScriptBundle("~/scripts/common").Include(
                "~/Scripts/Libs/jQuery/jquery-*",
                "~/Scripts/Libs/Fanex.Grid/Fanex.Grid.js",
                "~/Scripts/Libs/Fanex.Grid/Fanex.Pagination.js",
                "~/Scripts/Libs/Fanex.Grid/moment.js",
                "~/Scripts/Libs/Fanex.Grid/numeral.js",
                "~/Scripts/Libs/DatePicker/jquery.nexDatePicker.js",
                "~/Scripts/Libs/msgbox/msgbox-0.0.1.js",
                "~/Scripts/Libs/Tipsy/jquery.tipsy.js",
                "~/Scripts/Libs/others/Namespace.js",
                "~/Scripts/Miscellany.js",
                "~/Scripts/Layout.js"));
        }

        /// <summary>
        /// Registers the common bundles.
        /// </summary>
        /// <param name="bundles">The bundle collection.</param>
        public static void RegisterOpenCloseMemberBundles(BundleCollection bundles)
        {
            bundles.AddAndInclude(
                "~/styles/openCloseMember",
                "~/Content/Sites/openCloseMember.less");

            bundles.Add(new ScriptBundle("~/scripts/openCloseMember").Include(
                "~/Scripts/Sites/OpenCloseMember.js"));
        }
    }

    /// <summary>
    /// Bundle Collection extension.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Need to review here.")]
    internal static class BundleExtensions
    {
        /// <summary>
        /// Creates a bundle, include the virtual paths to the created bundle,
        /// and add it to the bundle collection.
        /// </summary>
        /// <param name="bundles">The bundle collection which the virtual paths are included.</param>
        /// <param name="bundleVirtualPath">Virtual path of the created bundle.</param>
        /// <param name="virtualPaths">The virtual paths that needed to add to bundle collection.</param>
        public static void AddAndInclude(
            this BundleCollection bundles,
            string bundleVirtualPath,
            params string[] virtualPaths)
        {
            bundles.Add(new Bundle(bundleVirtualPath)
            {
                Transforms = { new CssTransformer(), new CssMinify() },
                Orderer = new NullOrderer()
            }.Include(virtualPaths));
        }

        /// <summary>
        /// Gets relative path from absolute path.
        /// </summary>
        /// <param name="context">The context using to get the path.</param>
        /// <param name="path">The absolute path.</param>
        /// <returns>The relative path.</returns>
        public static string RelativeFromAbsolutePath(this HttpContextBase context, string path)
        {
            var request = context.Request;
            var applicationPath = request.PhysicalApplicationPath;
            var virtualDir = request.ApplicationPath;
            virtualDir = virtualDir == "/" ? virtualDir : (virtualDir + "/");
            return path.Replace(applicationPath, virtualDir).Replace(@"\", "/");
        }
    }
}