// --------------------------------------------------------------------------
// <copyright file="UserInfoController.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.28">Add properties _currentContext</change>
//   <change who="Jason.Ly" date="2014.4.14">Move out of User area.</change>
// </history>

namespace Fanex.NAP.CMS.Controllers
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using NAP.Core.Configuration;
    using NAP.Core.Models.ParameterObject;
    using ViewModels.UserInfo;

    /// <summary>
    /// User controller.
    /// </summary>
    public class UserInfoController : BaseController
    {
        private const string FormTokenKey = "_SecureChangePasswordFormToken";
        private readonly IUserService _userService;
        private readonly HashAlgorithm _hashAlgorithm;
        private readonly ISecurityService _securityService;

        public UserInfoController()
            : this(null, null)
        {
        }

        public UserInfoController(IUserService userService, ISecurityService securityService)
        {
            _userService = userService ?? new UserService();
            _hashAlgorithm = new SHA256Managed();
            _securityService = securityService ?? new SecurityService();
        }

        public string FormToken
        {
            get
            {
                if (Session[FormTokenKey] == null)
                {
                    return string.Empty;
                }

                return Session[FormTokenKey].ToString();
            }

            set
            {
                Session[FormTokenKey] = value;
            }
        }

        #region Change Password

        /// <summary>
        /// Redirect to changing password form.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult ChangePassword()
        {
            GenerateToken();

            return View();
        }

        /// <summary>
        /// Change password action.
        /// </summary>
        /// <param name="criteria">Criteria using to update password.</param>
        /// <param name="token">Changing password token.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel viewModel, string token)
        {
            if (CheckValidToken(token))
            {
                var user = _userService.Get(CurrentSession.UserId);
                var hashPassword = GetHash(user.Password + token);

                if (hashPassword == viewModel.OldUserPwd)
                {
                    ViewBag.Success = _securityService.UpdatePassword(NapSettings.Settings.SiteId, user.Id, viewModel.UserPwd);
                }
                else
                {
                    ViewBag.Message = ViewResources.Wrong_password;
                }
            }

            GenerateToken();

            return View(viewModel);
        }

        #endregion Change Password

        #region Profile

        /// <summary>
        /// Redirect to profile page.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult UserProfile()
        {
            var user = this._userService.Get(CurrentSession.UserId);
            var viewModel = new ProfileViewModel
            {
                CustId = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            return View(viewModel);
        }

        /// <summary>
        /// Update user profile.
        /// </summary>
        /// <param name="criteria">Criteria using to update user profile.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult UserProfile(ProfileViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Success = _securityService.UpdateProfile(
                    NapSettings.Settings.SiteId,
                    new UpdateUserInfoParameter
                    {
                        CustId = CurrentSession.UserId,
                        Username = CurrentSession.Username,
                        FirstName = viewModel.FirstName ?? string.Empty,
                        LastName = viewModel.LastName ?? string.Empty,
                        Email = viewModel.Email ?? string.Empty
                    });
            }

            return View(viewModel);
        }

        #endregion Profile

        #region Helpers

        /// <summary>
        /// Generates the token for login form.
        /// </summary>
        private void GenerateToken()
        {
            string token = Guid.NewGuid().ToString("N");

            FormToken = token;
            ViewBag.ChangePasswordToken = token;
            AddTokenToCookie(token);
        }

        /// <summary>
        /// Adds the login token to cookie.
        /// </summary>
        /// <param name="token">The changing password token.</param>
        private void AddTokenToCookie(string token)
        {
            var responseCookie = new HttpCookie(FormTokenKey)
            {
                HttpOnly = true,
                Value = token
            };

            // If we are using SSL, the cookie should be set to secure to
            // prevent it from being sent over HTTP connections
            if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
            {
                responseCookie.Secure = true;
            }

            // Add the cookie to the response
            Response.Cookies.Set(responseCookie);
        }

        /// <summary>
        /// Checks if the token is valid or not.
        /// </summary>
        /// <param name="token">The changing password token.</param>
        /// <returns>Boolean value indicates if the login token is valid or not.</returns>
        private bool CheckValidToken(string token)
        {
            return !string.IsNullOrWhiteSpace(token)
                && FormToken == token
                && Request.Cookies[FormTokenKey] != null
                && token == Request.Cookies[FormTokenKey].Value;
        }

        /// <summary>
        /// Gets the hash value of input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Hash value of input.</returns>
        private string GetHash(string input)
        {
            var encrypted = string.Empty;

            if (!string.IsNullOrWhiteSpace(input))
            {
                var hashData = _hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

                encrypted = BitConverter.ToString(hashData).Replace("-", string.Empty).ToLowerInvariant();
            }

            return encrypted;
        }

        #endregion Helpers
    }
}