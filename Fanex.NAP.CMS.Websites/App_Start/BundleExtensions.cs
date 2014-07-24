// --------------------------------------------------------------------------
// <copyright file="BundleExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.7.22">Created</change>
// </history>

namespace Fanex.NAP.CMS.Websites
{
    using System.Web;
    using System.Web.Optimization;
    using BundleTransformer.Core.Orderers;
    using BundleTransformer.Core.Transformers;

    /// <summary>
    /// Bundle Collection extension.
    /// </summary>
    public static class BundleExtensions
    {
        /// <summary>
        /// Creates a bundle, include the virtual paths to the created bundle,
        /// and add it to the bundle collection.
        /// </summary>
        /// <param name="bundles">The bundle collection which the virtual paths are included.</param>
        /// <param name="bundleVirtualPath">Virtual path of the created bundle.</param>
        /// <param name="virtualPaths">The virtual paths that needed to add to bundle collection.</param>
        public static void AddAndInclude(this BundleCollection bundles, string bundleVirtualPath, params string[] virtualPaths)
        {
            bundles.Add(new Bundle(bundleVirtualPath)
            {
                Transforms = { new StyleTransformer(), new CssMinify() },
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
            HttpRequestBase request = context.Request;
            string applicationPath = request.PhysicalApplicationPath;
            string virtualDir = request.ApplicationPath;

            virtualDir = virtualDir == "/" ? virtualDir : (virtualDir + "/");

            return path.Replace(applicationPath, virtualDir).Replace(@"\", "/");
        }
    }
}