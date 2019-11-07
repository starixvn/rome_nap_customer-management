using System;
using System.Web;
using System.Web.Security;
using Fanex.NAP.Core.Web.Auth;
using Fanex.NAP.Core.Web.Session;

namespace Fanex.NAP.CMS.Controllers
{
    public static class AuthInfoUtils
    {
        public static void CreateAuthInfo(ICurrentSession session, HttpContextBase httpContext)
        {
            var authInfo = new AuthInfo
            {
                UserId = session.UserId,
                OnUserId = session.OnUserId,
            };

            var cookiePath = httpContext.Request.ApplicationPath;
            var authTicket = new FormsAuthenticationTicket(
                1,
                session.Username,
                DateTime.Now,
                DateTime.Now.AddMinutes(20000),
                false,
                authInfo.ToXml(),
                cookiePath);

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Path = cookiePath,
                Name = FormsAuthentication.FormsCookieName
            };

            httpContext.Response.Cookies.Add(authCookie);
        }
    }
}
