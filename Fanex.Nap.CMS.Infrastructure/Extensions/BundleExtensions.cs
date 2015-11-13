
namespace Fanex.Nap.CMS.Infrastructure.Extensions
{
    using System.Web;
    using System.Web.Optimization;
    using BundleTransformer.Core.Orderers;
    using BundleTransformer.Core.Transformers;

    public static class BundleExtensions
    {
        public static void AddAndInclude(this BundleCollection bundles, string bundleVirtualPath, params string[] virtualPaths)
        {
            bundles.Add(new Bundle(bundleVirtualPath)
            {
                Transforms = { new StyleTransformer(), new CssMinify() },
                Orderer = new NullOrderer()
            }.Include(virtualPaths));
        }

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
