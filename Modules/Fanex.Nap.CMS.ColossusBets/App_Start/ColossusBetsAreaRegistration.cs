namespace Fanex.Nap.CMS.ColossusBets
{
	using System.Web.Mvc;
	using System.Web.Optimization;

	public class ColossusBetsAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "ColossusBets";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				name: AreaName + "_Default",
				url: AreaName + "/{controller}/{action}/{id}",
				defaults: new { controller = "BonusTicket", action = "Index", id = UrlParameter.Optional },
				namespaces: new string[] { "Fanex.Nap.CMS.ColossusBets.Controllers" });

			BundlesConfig.RegisterBundles(BundleTable.Bundles);
		}
	}
}
