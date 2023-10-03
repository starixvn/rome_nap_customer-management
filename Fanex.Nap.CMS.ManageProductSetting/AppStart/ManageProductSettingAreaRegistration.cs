namespace Fanex.Nap.CMS.ManageProductSetting.AppStart
{
    using System.Web.Mvc;
    public class ManageProductSettingAreaRegistration : AreaRegistration
    {
        public override string AreaName => "ManageProductSetting";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ManageProductSetting",
                "ManageProductSetting/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Fanex.Nap.CMS.ManageProductSetting.Controllers" });

        }
    }
}