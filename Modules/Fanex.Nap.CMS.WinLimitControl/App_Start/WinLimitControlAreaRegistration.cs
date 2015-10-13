namespace Fanex.Nap.CMS.WinLimitControl
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    public class WinLimitControlAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "WinLimitControl";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "WinLimitControl_default",
                "WinLimitControl/{controller}/{action}/{id}",
                new { controller = "WinLimitMonitor", action = "Index", id = UrlParameter.Optional },
                new[] { "Fanex.Nap.CMS.WinLimitControl.Controllers" });

            WinLimitControlBundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
