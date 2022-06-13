namespace Fanex.Nap.CMS.ManageChatbots.AppStart
{
    using System.Web.Optimization;
    using BundleTransformer.Core.Orderers;
    using BundleTransformer.Core.Transformers;

    public static class ManageChatbotsBundleConfig
    {
        public const string ChatbotMappingsScripts = "~/areas/mangechatbots/chatbotmappingscripts";
        public const string ChatbotMappingStyles = "~/areas/mangechatbots/chatbotmappingstyles";

        public const string ChatbotHistoryScripts = "~/areas/mangechatbots/chatbothistoryscripts";
        public const string ChatbotHistoryStyles = "~/areas/mangechatbots/chatbothistorystyles";

        public const string NotificationListSrcipts = "~/areas/mangechatbots/notificationlistscripts";
        public const string NotificationListStyles = "~/areas/mangechatbots/notificationliststyles";

        public static void RegisterAll(BundleCollection bundles)
        {
            RegisterChatbotMappingBundles(bundles);
            RegisterChatbotHistoryBundles(bundles);
            RegisterNotificationBundles(bundles);
        }

        private static void RegisterChatbotMappingBundles(BundleCollection bundles)
        {
            bundles
                .Add(new ScriptBundle(ChatbotMappingsScripts)
                    .Include(
                        "~/Scripts/Libs/Fanex.Multiselect/jquery.multiselect.js",
                        "~/Scripts/Libs/Popup/dialogbox.js",
                        "~/Areas/MangeChatbots/Scripts/ChatbotMapping/_ConfigChatbot.js",
                        "~/Areas/MangeChatbots/Scripts/ChatbotMapping/ChatbotMapping.js"
                    )
                );
            AddStyle(
                bundles,
                ChatbotMappingStyles,
                "~/Scripts/Libs/Fanex.Multiselect/jquery.multiselect.css",
                "~/Areas/MangeChatbots/Styles/ChatbotMapping/ChatbotMapping.less");
        }

        private static void RegisterChatbotHistoryBundles(BundleCollection bundles)
        {
            bundles
                .Add(new ScriptBundle(ChatbotHistoryScripts)
                    .Include("~/Areas/MangeChatbots/Scripts/ChatbotHistory/ChatbotHistory.js"));

            AddStyle(
                bundles,
                ChatbotHistoryStyles,
                "~/Areas/MangeChatbots/Styles/ChatbotHistory/ChatbotHistory.less");
        }

        private static void RegisterNotificationBundles(BundleCollection bundles)
        {
            bundles
                .Add(new ScriptBundle(NotificationListSrcipts)
                .Include(
                    "~/Scripts/Libs/Fanex.Multiselect/jquery.multiselect.js",
                    "~/Areas/MangeChatbots/Scripts/NotificationList/NotificationList.js"));

            AddStyle(
                bundles,
                NotificationListStyles,
                "~/Scripts/Libs/Fanex.Multiselect/jquery.multiselect.css",
                "~/Areas/MangeChatbots/Styles/NotificationList/NotificationList.less");
        }
        
        private static void AddStyle(BundleCollection bundles, string bundleVirtualPath, params string[] virtualPaths)
            => bundles.Add(new Bundle(bundleVirtualPath)
            {
                Transforms = { new StyleTransformer(), new CssMinify() },
                Orderer = new NullOrderer()
            }.Include(virtualPaths));
    }
}