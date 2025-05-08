using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Fanex.NAP.Client;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Models;
using Fanex.NAP.Client.Models.ParameterObject;
using Fanex.NAP.Client.Services;
using Fanex.NAP.SignIn.Models;
using Fanex.NAP.SignIn.ViewModel;
using ONEbook.UM.AspNetMVC.Filters;
using ONEbook.UM.Services;

namespace Fanex.NAP.SignIn
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly INapSession _napSession;
        private readonly ISecurityService _securityService;
        private readonly IGroupService _groupService;
        private readonly IUserService _userService;
        private readonly IMaintenanceService _maintenanceService;
        private readonly INapSettings _napSettings;

        private const string FormTokenName = "__SecureFormToken";
        private const string OtpSite = "NAP";

        public AuthController(
            INapSession napSession,
            ISecurityService securityService,
            IGroupService groupService,
            IUserService userService,
            IMaintenanceService maintenanceService,
            INapSettings napSettings)
        {
            _napSession = napSession;
            _securityService = securityService;
            _groupService = groupService;
            _userService = userService;
            _maintenanceService = maintenanceService;
            _napSettings = napSettings;
        }

        private string FormToken
        {
            get
            {
                var token = Session[FormTokenName]?.ToString();
                return string.IsNullOrEmpty(token) ? string.Empty : token;
            }
            set => Session[FormTokenName] = value;
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (!_napSettings.EnableDirectLogin)
            {
                return Redirect(_napSettings.HubUrl);
            }

            Session["OtpUserId"] = 0;
            GenerateNewToken();
            var indexViewModel = new IndexViewModel { FormToken = FormToken };
            return View("Index", indexViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Index(string username, string password, string loginToken = "")
        {
            try
            {
                var user = await GetValidUser(username, password, loginToken);
                return await OtpLoginFlow(user);
            }
            catch (Exception e)
            {
                GenerateNewToken();

                var indexViewModel = new IndexViewModel
                {
                    Message = e.Message,
                    FormToken = FormToken
                };

                return View("Index", indexViewModel);
            }
        }

        private async Task<User> GetValidUser(string username, string password, string loginToken = "")
        {
            if (string.IsNullOrEmpty(FormToken))
            {
                throw new InvalidOperationException("Form token is empty.");
            }

            if (!IsValidUserPassword(username, password, loginToken))
            {
                throw new InvalidOperationException("Invalid Username or Password");
            }

            var user = await _userService.GetAsync(username.Trim());

            if (!IsAuthorizedUser(user))
            {
                throw new InvalidOperationException("You have no authorization");
            }

            if (!IsActivatedUser(user))
            {
                throw new InvalidOperationException("Your account was suspended");
            }

            return user;
        }

        private bool IsValidUserPassword(string username, string password, string token)
        {
            if (!IsValidToken(token)
                || string.IsNullOrEmpty(username)
                || string.IsNullOrEmpty(password))
            {
                return false;
            }

            var user = _userService.Get(username.Trim());

            if (user == null || user.Id == 0)
            {
                return false;
            }

            var hashPassword = ToSHA256(user.Password + token);
            return password == hashPassword;
        }

        private bool IsAuthorizedUser(User user)
        {
            var group = user.GroupId == 0 ? null : _groupService.Get(user.GroupId);

            var siteLevelList = _userService.GetSiteLevel(user.Id, _napSettings.Environment);

            return (group == null || group.IsActive) &&
                   siteLevelList.Any(x => x.IsActive);
        }

        private bool IsActivatedUser(User user)
        {
            var siteLevelList = _userService.GetSiteLevel(user.Id, _napSettings.Environment);

            var userIsActiveOnThisSite = false;

            foreach (var siteLevel in siteLevelList)
            {
                if (siteLevel.SiteId == _napSettings.SiteId && siteLevel.IsActive)
                {
                    userIsActiveOnThisSite = true;
                }
            }

            return userIsActiveOnThisSite;
        }

        [HttpGet]
        public async Task<ActionResult> LogOut(string redirectUrl = "")
        {
            await DoSignOut();

            redirectUrl = string.IsNullOrEmpty(redirectUrl) ? "/Auth" : redirectUrl;

            return Redirect(redirectUrl);
        }

        public ActionResult ReCheckIn()
        {
            if (!_napSettings.EnableReCheckIn)
            {
                return Json(new
                {
                    IsKickedOut = 0
                });
            }

            var isKickedOut = _napSession.OnUserId <= 0 || _securityService.ReCheckIn(_napSession.OnUserId, _napSettings.SiteId) > 0 ? 1 : 0;

            return Json(new
            {
                _napSession.OnUserId,
                IsKickedOut = isKickedOut
            });
        }

        private async Task DoSignOut()
        {
            if (_napSession.OnUserId != 0)
            {
                await _securityService.LogoutAsync(_napSession.OnUserId);
            }

            _napSession.ClearAll();
            FormsAuthentication.SignOut();
            _napSession.ClearAll();
        }

        private void CreateUserMenu(User user)
        {
            _napSession.MenuItemList = _userService.GetMenuItem(
                    user.Id,
                    _napSettings.SiteId,
                    _napSettings.Environment)
                .ToList();
        }

        private async Task CreateAuthInfo(User user)
        {
            var userData = new
            {
                user.Username,
                user.Id,
                // Add any other user data you need
            };

            var ticket = new FormsAuthenticationTicket(
                1,                                  // version
                user.Username,                     // user name
                DateTime.Now,                      // issue time
                DateTime.Now.AddHours(24),         // expiration
                false,                             // persistent
                Newtonsoft.Json.JsonConvert.SerializeObject(userData), // user data
                FormsAuthentication.FormsCookiePath // cookie path
            );

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(cookie);
        }

        private LoginParameter SetUserCheckInCriteria(int userId)
        {
            return new LoginParameter
            {
                UserId = userId,
                SiteId = _napSettings.SiteId,
                LoginDate = DateTime.Now,
                NetSessionId = Session.SessionID,
                Ip = GetClientIP(),
                BrowserName = Request.Headers["User-Agent"].ToString(),
                CountryCode = GetDefaultCountryCode(),
                Domain = Request.Url.Host,
                Platform = Environment.OSVersion.Platform.ToString()
            };
        }

        private string GetClientIP()
        {
            var clientIP = Request.Headers["x-forwarded-for"]?.ToString();
            if (string.IsNullOrWhiteSpace(clientIP))
            {
                clientIP = Request.ServerVariables["REMOTE_ADDR"]?.ToString();
            }

            return clientIP;
        }

        private static string GetDefaultCountryCode()
        {
            var culture = CultureInfo.CreateSpecificCulture("en");

            return new RegionInfo(culture.LCID).GeoId.ToString(CultureInfo.CurrentCulture);
        }

        private async Task SaveUserSession(User user, string userName, int onUserId)
        {
            _napSession.Level = await _userService.GetUserLevelAsync(user.Id, _napSettings.SiteId);
            _napSession.UserId = user.Id;
            _napSession.UserName = userName;
            _napSession.OnUserId = onUserId;

            CreateUserMenu(user);
            await CreateAuthInfo(user);
        }

        private static string ToSHA256(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Hey guy! I do not want to hash an empty or white space string");
            }

            using (var hashAlgorithm = SHA256.Create())
            {
                var hashData = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
                var encrypted = BitConverter.ToString(hashData).Replace("-", string.Empty).ToLower();
                return encrypted;
            }
        }

        private void AddTokenToCookie(string loginToken)
        {
            var cookie = new HttpCookie(FormTokenName, loginToken)
            {
                Path = "/"
            };
            Response.Cookies.Add(cookie);
        }

        private bool IsValidToken(string loginToken)
        {
            if (string.IsNullOrWhiteSpace(loginToken))
                return false;

            var cookie = Request.Cookies[FormTokenName];
            return FormToken == loginToken && 
                   cookie != null && 
                   loginToken == cookie.Value;
        }

        private void GenerateNewToken()
        {
            var loginToken = Guid.NewGuid().ToString("N");
            FormToken = loginToken;
            AddTokenToCookie(loginToken);
        }

        [SkipUMFilter]
        public async Task<ActionResult> Offline()
        {
            var isDownTime = await _maintenanceService.IsUnderMaintenanceAsync();
            if (!isDownTime)
            {
                return Redirect("/");
            }

            var downTime = await _maintenanceService.GetUnderMaintenanceTimeAsync();

            var viewModel = new MaintenanceViewModel { UmtFrom = downTime.From, UmtTo = downTime.To };

            return View("Offline", viewModel);
        }

        public ActionResult AccessDenied()
        {
            return View("AccessDenied");
        }

        public ActionResult Error()
        {
            return View("Error");
        }

        [HttpGet]
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View("ChangePassword", new UpdatePasswordModel());
        }

        [HttpPost]
        [Authorize]
        public JsonResult ChangePassword(UpdatePasswordModel criteria)
        {
            var user = _userService.Get(_napSession.UserId);
            var oldPwd = criteria.OldUserPwd;

            if (oldPwd.Equals(user.Password))
            {
                var password = criteria.UserPwd;
                _securityService.UpdatePassword(_napSettings.SiteId, user.Id, password);
            }
            else
            {
                return Json(new { ErrorMsg = "Wrong password" });
            }

            return Json(new { ErrorMsg = "" });
        }

        #region SSO
        private const int TimeExpirationInSecond = 60;

        [HttpGet]
        public async Task<ActionResult> SSO(string userId, string onuserId, string time, string redirectUrl = "")
        {
            var logged = await IsSSOAuthenticated(time);
            if (!logged)
            {
                return Redirect("/Auth/Index");
            }

            var userIdDecrypt = Cryptography.DecryptPassword(userId.Replace(" ", "+"));
            var onuserIdDecrypt = Cryptography.DecryptPassword(onuserId.Replace(" ", "+"));

            var criteria = CreateSsoLoginParameter(userIdDecrypt, onuserIdDecrypt);
            var ssoLoginOnUserId = await _securityService.SSOLoginAsync(criteria);

            await CreateSSOAuth(userId, ssoLoginOnUserId);

            var nextUrl = string.IsNullOrEmpty(redirectUrl) ? "/" : redirectUrl;
            return Redirect(nextUrl);
        }

        public async Task<bool> IsSSOAuthenticated(string time)
        {
            var currentTime = await _securityService.GetTimeAsync();
            var timeDecrypt = Cryptography.DecryptPassword(time.Replace(" ", "+"));
            var expiration = Convert.ToDateTime(currentTime) - Convert.ToDateTime(timeDecrypt);

            return expiration.TotalSeconds < TimeExpirationInSecond;
        }

        private async Task<bool> CreateSSOAuth(string userId, int onUserId)
        {
            var user = _userService.Get(Convert.ToInt32(userId));
            if (user == null)
            {
                return false;
            }

            var criteria = new LoginParameter
            {
                UserId = user.Id,
                SiteId = _napSettings.SiteId,
                LoginDate = DateTime.Now,
                NetSessionId = Session.SessionID,
                Ip = GetClientIP(),
                BrowserName = Request.Headers["User-Agent"].ToString(),
                CountryCode = GetDefaultCountryCode(),
                Domain = Request.Url.Host,
                Platform = Environment.OSVersion.Platform.ToString()
            };

            _napSession.UserId = user.Id;
            _napSession.UserName = user.Username;
            _napSession.OnUserId = onUserId;
            _napSession.Level = await _userService.GetUserLevelAsync(user.Id, _napSettings.SiteId);

            if (_napSettings.SiteId != 0)
            {
                _napSession.MenuItemList = await _userService.GetMenuItemAsync(user.Id,
                    _napSettings.SiteId,
                    _napSettings.Environment) as IList<MenuItem>;
            }

            await CreateAuthInfo(user);

            return true;
        }

        private SSOLoginParameter CreateSsoLoginParameter(string userId, string onuserId)
        {
            return new SSOLoginParameter
            {
                UserId = Convert.ToInt32(userId),
                SiteId = _napSettings.SiteId,
                CheckOnUserId = Convert.ToInt32(onuserId),
                OnUserId = 0,
                LoginDate = DateTime.Now,
                NetSessionId = Session.SessionID,
                IP = GetClientIP(),
                BrowserName = Request.Headers["User-Agent"].ToString(),
                CountryCode = GetDefaultCountryCode(),
                Domain = Request.Url.Host,
                Platform = Environment.OSVersion.Platform.ToString()
            };
        }

        [HttpGet]
        [SkipUMFilter]
        public async Task<ActionResult> Propagation(string userId, string onuserId, string time, string redirectUrl = "")
        {
            return await SSO(userId, onuserId, time, redirectUrl);
        }

        #endregion

        #region LoginFlow

        private async Task<ActionResult> OtpLoginFlow(User user)
        {
            var ignoredOtp = !_napSettings.EnableOtp
                             || await _userService.IgnoredOtpAsync(user.Id, OtpSite);

            if (ignoredOtp)
            {
                return await FirstLoginFlow(user);
            }

            Session["OtpUserId"] = user.Id;
            return View("OtpLoginFlow");
        }

        [HttpPost]
        public async Task<ActionResult> OtpLoginFlow(string otp)
        {
            ViewBag.Message = string.Empty;
            var otpUserId = Session["OtpUserId"] as int?;
            var userId = otpUserId ?? 0;

            if (userId == 0)
            {
                return RedirectToAction("Index");
            }

            var validOtp = !string.IsNullOrEmpty(otp)
                           && await _userService.VerifyOtpAsync(userId, OtpSite, otp);

            if (validOtp)
            {
                Session["OtpUserId"] = null;
                var user = await _userService.GetAsync(userId);
                return await FirstLoginFlow(user);
            }

            ViewBag.Message = "Invalid OTP. Please try again";

            return View("OtpLoginFlow");
        }

        private async Task<ActionResult> FirstLoginFlow(User user)
        {
            if (user.PasswordExpiry.Year != 1900)
            {
                return await DefaultLoginFlow(user);
            }
            Session["FirstLoginUserId"] = user.Id;
            return View("FirstLoginFlow", new UpdatePasswordModel { Username = user.Username });
        }

        [HttpPost]
        public async Task<JsonResult> FirstLoginFlow(UpdatePasswordModel criteria)
        {
            var firstLoginUserId = Session["FirstLoginUserId"] as int?;
            var userId = firstLoginUserId ?? 0;
            if (userId == 0)
            {
                return Json(new { ErrorMsg = "Invalid Flow" });
            }

            var user = await _userService.GetAsync(userId);
            var oldPwd = criteria.OldUserPwd;

            if (oldPwd.Equals(user.Password))
            {
                var password = criteria.UserPwd;
                await _securityService.UpdatePasswordAsync(_napSettings.SiteId, user.Id, password);
                Session["FirstLoginUserId"] = null;
                await DefaultLoginFlow(user);
            }
            else
            {
                return Json(new { ErrorMsg = "Wrong password" });
            }

            return Json(new { ErrorMsg = "" });
        }

        #endregion

        private async Task<ActionResult> DefaultLoginFlow(User user)
        {
            var criteria = new LoginParameter
            {
                UserId = user.Id,
                SiteId = _napSettings.SiteId,
                LoginDate = DateTime.Now,
                NetSessionId = Session.SessionID,
                Ip = GetClientIP(),
                BrowserName = Request.Headers["User-Agent"].ToString(),
                CountryCode = GetDefaultCountryCode(),
                Domain = Request.Url.Host,
                Platform = Environment.OSVersion.Platform.ToString()
            };

            var onUserId = await _securityService.LoginAsync(criteria);

            await SaveUserSession(user, user.Username, onUserId);
            return Redirect("/");
        }

        public ActionResult WhoAmI()
        {
            return Content($"Environment:{Environment.GetEnvironmentVariable("ASPNETCORE_APPL_PATH")}");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult UpdatePassword(UpdatePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // ... existing password update logic ...
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult UpdatePasswordFirstLogin(UpdatePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // ... existing first login password update logic ...
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
    }
}