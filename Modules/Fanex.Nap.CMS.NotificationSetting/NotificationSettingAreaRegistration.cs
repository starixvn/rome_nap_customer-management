// -----------------------------------------------------------------------
// <copyright file="NotificationSettingAreaRegistration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Nap.CMS.NotificationSetting
{
    using System.Web.Mvc;

    /// <summary>
    /// Register area.
    /// </summary>
    public class NotificationSettingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "NotificationSetting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "NotificationSetting_default",
                "NotificationSetting/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Fanex.Nap.CMS.NotificationSetting.Controllers" });
        }
    }
}