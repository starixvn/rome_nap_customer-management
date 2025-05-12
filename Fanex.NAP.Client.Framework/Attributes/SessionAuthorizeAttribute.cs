using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fanex.NAP.Client.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class SessionAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext?.Request?.HttpMethod == "OPTIONS")
            {
                return true;
            }

            if (!httpContext.User?.Identity?.IsAuthenticated ?? false)
            {
                return false;
            }

            var currentSession = DependencyResolver.Current.GetService<INapSession>();
            return !string.IsNullOrEmpty(currentSession.UserName) && currentSession.UserId > 0;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = 401;
                filterContext.HttpContext.Response.End();
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}