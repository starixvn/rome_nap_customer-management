// --------------------------------------------------------------------------
// <copyright file="TurnoverControlAreaRegistration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.Nap.CMS.TurnoverControl
{
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TurnoverControlAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "TurnoverControl";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "TurnoverControl_default",
                "TurnoverControl/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Fanex.Nap.CMS.TurnoverControl.Controllers" });
        }
    }
}
