namespace Fanex.Nap.CMS.PayoutManagement
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    public class PayoutManagementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PayoutManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: AreaName + "_Default",
                url: AreaName + "/{controller}/{action}/{id}",
                defaults: new { controller = "Payout", action = "Index", id = UrlParameter.Optional },
                namespaces: new [] { "Fanex.Nap.CMS.PayoutManagement.Controllers" });

            BundlesConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
