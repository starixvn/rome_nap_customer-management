using System;

namespace Fanex.Nap.CMS.Infrastructure.Constants
{
    using System.Collections.Generic;

    public static class LanguageNameConstants
    {
        private static Dictionary<string, string> _roleName = new Dictionary<string, string>
        {
            { "en-US", "English" },
            { "zh-Hans", "Chinese" },
            { "zh-Hant", "Taiwan" },
            { "vi-VN", "Vietnamese" },
            { "th-TH", "Thai" },
            { "ja-JP", "Japanese" },
            { "ko-KR", "Korean" }
        };

        public static string GetLanguageName(string languageKey)
        {
            try
            {
                return _roleName[languageKey];
            }
            catch 
            {
                return "";
            }
        }

        public const string English = "en-US";
        public const string Chinese = "zh-CN";
        public const string Taiwan = "zh-TW";
        public const string Vietnamese = "vi-VN";
        public const string Thai = "th-TH";
        public const string Japanese = "ja-JP";
        public const string Korean = "ko-KR";
    }
}