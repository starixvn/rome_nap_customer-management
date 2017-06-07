using System;
using System.Web;
using System.Web.Security;
using Fanex.NAP.Core.Model;
using Fanex.NAP.Core.Web.Auth;
using Fanex.NAP.Core.Web.Context;

namespace Fanex.NAP.CMS.Controllers
{
    public static class AuthInfoUtils
    {
        public static void CreateAuthInfo(User user, int onuserId, ICurrentContext currentContext, HttpResponseBase response)
        {
            var currentDomain = currentContext.GetCurrentDomain();

            var authInfo = new AuthInfo
            {
                UserId = user.Id,
                OnUserId = onuserId,
            };

            var authTicket = new FormsAuthenticationTicket(
                1,
                user.Username,
                DateTime.Now,
                DateTime.Now.AddMinutes(20000),
                false,
                authInfo.ToXml(),
                currentDomain.Path);

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Path = currentDomain.Path,
                Name = FormsAuthentication.FormsCookieName
            };

            response.Cookies.Add(authCookie);
        }
    }
}
