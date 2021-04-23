namespace Fanex.Nap.CMS.Customers.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Customers.Services;
    using Fanex.Nap.CMS.Customers.ViewModels.VirtualRateEngine;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    public class VirtualRateEngineController : BaseController
    {
        private readonly IManageRobots2Service manageRobots2Service;

        public VirtualRateEngineController()
            : this(new ManageRobots2Service())
        {
        }

        internal VirtualRateEngineController(IManageRobots2Service manageRobots2Service)
        {
            this.manageRobots2Service = manageRobots2Service;
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.VirtualRateEngine, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            HasRight(CMSFunctionKeys.VirtualRateEngine, Constant.Policy.Host);
            HasRight(CMSFunctionKeys.FullPermission, Constant.Policy.Host);
            var sitesModel = manageRobots2Service.GetSites();

            var viewModel = new VirtualRateEngineIndexViewModel(
                HasRight(CMSFunctionKeys.VirtualRateEngine, Constant.Policy.Host),
                HasRight(CMSFunctionKeys.FullPermission, Constant.Policy.Host),
                sitesModel.Credits.Select(s => new IdNameModel { Id = s.SiteId, Name = s.SiteName }),
                sitesModel.Licensees.Select(s => new IdNameModel { Id = s.SiteId, Name = s.SiteName }));

            return View("~/Areas/Customers/Views/VirtualRateEngine/Index.cshtml", viewModel);
        }
    }
}
