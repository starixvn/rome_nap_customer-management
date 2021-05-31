namespace Fanex.Nap.CMS.Infrastructure.Extensions
{
    using System;
    using System.Text;

    public static class StringExtentions
    {
        public static string NewlinesToBr(this string text)
        {
            // \r: in Mac OS before X
            // \n: in Unix/Mac OS X
            // \r\n: in Windows
            StringBuilder sb = new StringBuilder(text);
            sb.Replace("\r", "\n");
            sb.Replace("\r\n", "\n");
            sb.Replace("\n", "<br/>");

            return sb.ToString();
        }

        public static string[] ToArray(this string text, char[] separator = null)
            => string.IsNullOrWhiteSpace(text)
            ? Array.Empty<string>()
            : text.Split(separator ?? new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
    }
}
