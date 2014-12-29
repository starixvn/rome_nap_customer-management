// -----------------------------------------------------------------------
// <copyright file="MailInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Nap.CMS.NotificationSetting.Model.TurnoverControlNotification
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("T4", "1.0.0.0")]
    public class MailInfo
    {
        public string SiteName { get; set; }

        public int SiteId { get; set; }

        public string ToList { get; set; }

        public string CCList { get; set; }

        public string BCCList { get; set; }

        public bool IsMailExist { get; set; }
    }
}