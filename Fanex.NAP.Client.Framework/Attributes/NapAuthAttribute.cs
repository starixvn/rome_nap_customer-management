using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Services;
using Fanex.NAP.Client.Helpers.Business;

namespace Fanex.NAP.Client.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class NapAuthAttribute : AuthorizeAttribute
    {
        public string FunctionKey { get; set; }

        public Constant.Policy Policy { get; set; }

        public int CacheDuration { get; set; } = 60;

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

            return UserHasValidFunctionPolicy(httpContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var napSettings = DependencyResolver.Current.GetService<INapSettings>();
            var unauthorizeUrl = napSettings.UnauthorizeUrl;
            filterContext.Result = new RedirectResult(string.IsNullOrEmpty(unauthorizeUrl)
                ? "~/Unauthorize"
                : unauthorizeUrl);
        }

        private bool UserHasValidFunctionPolicy(HttpContextBase httpContext)
        {
            var userService = DependencyResolver.Current.GetService<IUserService>();
            var napSettings = DependencyResolver.Current.GetService<INapSettings>();
            var currentSession = DependencyResolver.Current.GetService<INapSession>();
            var cacheKey = $"{currentSession.UserId}_{napSettings.SiteId}_{FunctionKey}";
            var policy = HttpRuntime.Cache[cacheKey];

            if (policy == null)
            {
                policy = userService.GetPermission(currentSession.UserId, napSettings.SiteId, FunctionKey);
                HttpRuntime.Cache.Insert(cacheKey, policy, null,
                    DateTime.Now.AddSeconds(CacheDuration),
                    Cache.NoSlidingExpiration);
            }

            return (int)policy < (int)Policy;
        }
    }
}