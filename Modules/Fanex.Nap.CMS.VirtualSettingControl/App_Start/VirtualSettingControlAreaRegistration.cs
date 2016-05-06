namespace Fanex.Nap.CMS.VirtualSettingControl
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    public class VirtualSettingControlAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "VirtualSettingControl";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "VirtualSettingControl_default",
                "VirtualSettingControl/{controller}/{action}/{id}",
                new { controller = "VirtualSettingMonitor", action = "Index", id = UrlParameter.Optional },
                new[] { "Fanex.Nap.CMS.VirtualSettingControl.Controllers" });

            VirtualSettingControlBundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
