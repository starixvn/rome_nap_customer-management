using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Fanex.NAP.Client.Attributes
{
    internal static class RequestHelper
    {
        public static void CloseConnection(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var response = filterContext.HttpContext.Response;
                const string messageContent = "<center><b>You are not authorized to access the requested resource!</b></center>";
                response.StatusCode = (int)HttpStatusCode.Forbidden;
                response.Write(messageContent);
                response.End();
            }
            else
            {
                FormsAuthentication.SignOut();
            }
        }
    }
}