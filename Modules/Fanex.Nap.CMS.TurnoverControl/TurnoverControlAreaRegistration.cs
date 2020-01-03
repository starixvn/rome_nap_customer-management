using System.Web.Mvc;

namespace Fanex.Nap.CMS.TurnoverControl
{
    public class TurnoverControlAreaRegistration : AreaRegistration
    {
        public override string AreaName
            => "TurnoverControl";

        public override void RegisterArea(AreaRegistrationContext context)
            => context.MapRoute(
                "TurnoverControl_default",
                "TurnoverControl/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Fanex.Nap.CMS.TurnoverControl.Controllers" });
    }
}
