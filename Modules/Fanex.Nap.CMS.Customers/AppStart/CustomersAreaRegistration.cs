namespace Fanex.Nap.CMS.Customers.AppStart
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    public class CustomersAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Customers";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CustomersDefault",
                "Customers/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Fanex.Nap.CMS.Customers.Controllers" });

            CustomersBundlesConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
