namespace Fanex.Nap.CMS.Customers.Controllers
{
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Customers.Criteria.ManageRobots2;
    using Fanex.Nap.CMS.Customers.Services;
    using Fanex.Nap.CMS.Customers.ViewModels.ManageRobots2;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    public class ManageRobots2Controller : BaseController
    {
        private readonly IManageRobots2Service service;

        public ManageRobots2Controller()
            : this(null)
        {
        }

        public ManageRobots2Controller(IManageRobots2Service service)
        {
            this.service = service ?? new ManageRobots2Service();
        }

        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.ManageRobots2, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var viewModel = new ManageRobots2IndexViewModel
            {
                HasEditRight = HasRight(CMSFunctionKeys.ManageRobots2, Constant.Policy.Host),
                Currencies = service.GetCurrencies(new GetCurrenciesCriteria())
            };

            return View("~/Areas/Customers/Views/ManageRobots2/Index.cshtml", viewModel);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ManageRobots2, Policy = Constant.Policy.Read)]
        public JsonResult GetSites()
        {
            return Json(service.GetSites());
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ManageRobots2, Policy = Constant.Policy.Read)]
        public JsonResult GetRobots(GetRobotsCriteria criteria)
        {
            return Json(service.GetRobots(criteria));
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ManageRobots2, Policy = Constant.Policy.Host)]
        public JsonResult DeleteRobot(DeleteRobotCriteria criteria)
        {
            criteria.UserId = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;

            return Json(service.DeleteRobot(criteria));
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ManageRobots2, Policy = Constant.Policy.Host)]
        public JsonResult AddRobot(AddRobotCriteria criteria)
        {
            criteria.UserId = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;

            return Json(service.AddRobot(criteria));
        }

        public void ExportRobots(GetRobotsCriteria criteria)
        {
            var report = service.GetRobots(criteria);
            var exporter = new RobotsExporter(report);
            exporter.ResponseStream();
        }
    }
}