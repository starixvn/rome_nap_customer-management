namespace Fanex.NAP.CMS.Controllers
{
    using System.Web.Mvc;
    using Exporter;
    using Models.StatusLog;
    using Nap.CMS.Infrastructure;
    using NAP.Core.Business;
    using NAP.Core.Web.Attribute;
    using Services.Implements;
    using Services.Interfaces;

    public class StatusLogController : BaseController
    {
        private readonly IStatusLogService _statusLogService;

        public StatusLogController() : this(null)
        {
        }

        public StatusLogController(IStatusLogService statusLogService)
        {
            _statusLogService = statusLogService ?? new StatusLogService();
        }

        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.CreditCollection, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.CreditCollection, Policy = Constant.Policy.Read)]
        public JsonResult Logs(LogFilter filter)
        {
            var result = _statusLogService.Logs(filter);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.CreditCollection, Policy = Constant.Policy.Read)]
        public void Export(LogFilter filter)
        {
            var result = _statusLogService.Logs(filter);

            var exporter = new StatusLogExporter(result);
            exporter.ResponseStream();
        }
    }
}
