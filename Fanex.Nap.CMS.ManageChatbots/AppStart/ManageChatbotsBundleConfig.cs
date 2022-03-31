namespace Fanex.Nap.CMS.ManageChatbots.AppStart
{
    using System.Web.Optimization;
    using BundleTransformer.Core.Orderers;
    using BundleTransformer.Core.Transformers;
    public class ManageChatbotsBundleConfig
    {
        public const string ChatbotMappingsScripts = "~/areas/mangechatbots/chatbotmappingscripts";
        public const string ChatbotMappingStyles = "~/areas/mangechatbots/chatbotmappingstyles";
        public static void RegisterAll(BundleCollection bundles)
        {
            RegisterChatbotMappingBundles(bundles);
        }
        private static void RegisterChatbotMappingBundles(BundleCollection bundles)
        {
            bundles
                .Add(new ScriptBundle(ChatbotMappingsScripts)
                    .Include(
                        "~/Scripts/Libs/Popup/dialogbox.js",
                        "~/Areas/MangeChatbots/Scripts/ChatbotMapping/ChatbotMapping.js"));
            AddStyle(
                bundles,
                ChatbotMappingStyles,
                "~/Areas/MangeChatbots/Styles/ChatbotMapping/ChatbotMapping.less");
        }
        
        private static void AddStyle(BundleCollection bundles, string bundleVirtualPath, params string[] virtualPaths)
            => bundles.Add(new Bundle(bundleVirtualPath)
            {
                Transforms = { new StyleTransformer(), new CssMinify() },
                Orderer = new NullOrderer()
            }.Include(virtualPaths));
    }
}
