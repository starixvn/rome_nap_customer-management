namespace Fanex.Nap.CMS.Infrastructure.Helpers
{
    using System;
    using System.Collections;
    using System.Text;
    using System.Threading;
    using System.Web;
    using Fanex.Logging;

    public static class Logs
    {
        public static void Error(Exception ex, HttpRequest httpRequest = null)
        {
            var request = httpRequest ?? HttpContext.Current.Request;
            string error = FullExceptionToString(ex, request);
            Error(error);
        }

        #region LogService members and Helper methods

        private static void Error(string message)
        {
            LogService.Error(message);
        }

        private static string FullExceptionToString(Exception ex, HttpRequest httpRequest)
        {
            return string.Join(
                "\r\n",
                ExceptionToString(ex, httpRequest),
                "EXCEPTION DATA:" + ExceptionDataToString(ex.Data),
                "BROWSER CAPABILITIES:" + BrowserInformation(httpRequest.Browser),
                SqlExceptionToString(ex));
        }

        private static string ExceptionToString(Exception ex, HttpRequest httpRequest)
        {
            string url = httpRequest.Url.OriginalString;
            string formString = httpRequest.Form.Count > 0 ? httpRequest.Form.ToString() : string.Empty;

            return string.Join(
                "\r\n",
                ex.Message,
                "URL: " + url,
                "REFERRER: " + httpRequest.UrlReferrer,
                "FORM: " + formString,
                "SOURCE: " + ex.Source,
                "TARGETSITE: " + ex.TargetSite,
                "DETAIL: " + ex,
                "CULTURE: " + Thread.CurrentThread.CurrentUICulture);
        }

        private static string SqlExceptionToString(Exception ex)
        {
            var sqlException = ex as System.Data.SqlClient.SqlException;

            if (sqlException != null)
            {
                return string.Join(
                    "\r\n",
                    "PROCEDURE: " + sqlException.Procedure,
                    "LINE NUMBER: " + sqlException.LineNumber,
                    "SERVER: " + sqlException.Server);
            }

            return string.Empty;
        }

        private static string ExceptionDataToString(IDictionary dictionary)
        {
            var message = new StringBuilder();

            if (dictionary != null)
            {
                foreach (var key in dictionary.Keys)
                {
                    message.AppendFormat("\r\n{0}: {1}", key, dictionary[key]);
                }
            }

            return message.ToString();
        }

        private static string BrowserInformation(HttpBrowserCapabilities browser)
        {
            if (browser == null)
            {
                return string.Empty;
            }

            return string.Join(
                "\r\n",
                "Type: " + browser.Type,
                "Name: " + browser.Browser,
                "Version: " + browser.Version,
                "Major Version: " + browser.MajorVersion,
                "Minor Version: " + browser.MinorVersion,
                "Platform: " + browser.Platform,
                "Is Beta: " + browser.Beta,
                "Is Crawler: " + browser.Crawler,
                "Is AOL: " + browser.AOL,
                "Is Win16: " + browser.Win16,
                "Is Win32: " + browser.Win32,
                "Supports Frames: " + browser.Frames,
                "Supports Tables: " + browser.Tables,
                "Supports Cookies: " + browser.Cookies);
        }

        #endregion ILogger Members
    }
}
