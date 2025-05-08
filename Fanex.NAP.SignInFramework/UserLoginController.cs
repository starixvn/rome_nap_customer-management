using System;
using System.Web.Mvc;

namespace Fanex.NAP.SignIn
{
    [AllowAnonymous]
    public class UserLoginController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult SSO(string userId, string onuserId, string time, Uri redirectUrl = null)
        {
            return RedirectToAction("SSO", "Auth", new { userId, onuserId, time, redirectUrl });
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return RedirectToAction("Index", "Auth");
        }
    }
}