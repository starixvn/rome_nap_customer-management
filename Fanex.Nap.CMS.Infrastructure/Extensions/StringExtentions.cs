namespace Fanex.Nap.CMS.Infrastructure.Extensions
{
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
    }
}
