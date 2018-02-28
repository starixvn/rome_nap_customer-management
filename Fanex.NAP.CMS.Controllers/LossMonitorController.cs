namespace Fanex.NAP.CMS.Controllers
{
    using System.Web.Mvc;
    using Core;
    using Criteria.LossMonitor;
    using NAP.Core.Business;
    using NAP.Core.Web.Attribute;
    using Services.Implements;
    using Services.Interfaces;

    public class LossMonitorController : BaseController
    {
        private readonly ILossMonitorService _lossMonitorService;

        public LossMonitorController()
            : this(null)
        {
        }

        public LossMonitorController(ILossMonitorService lossMonitorService)
        {
            _lossMonitorService = lossMonitorService ?? new LossMonitorService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.LossMonitor, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            ViewData["HostPermission"] = HasRight(CMSFunctionKeys.LossMonitor, Constant.Policy.Host);

            return View();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.LossMonitor, Policy = Constant.Policy.Read)]
        public JsonResult GetSMALossReport()
        {
            var result = _lossMonitorService.GetCustomerLoss();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.LossMonitor, Policy = Constant.Policy.Read)]
        public JsonResult GetGroupDownlines(GetGroupDownlinesCriteria criteria)
        {
            var result = _lossMonitorService.GetGroupDownlines(criteria);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.LossMonitor, Policy = Constant.Policy.Host)]
        public JsonResult Add(InsertSMACriteria criteria)
        {
            criteria.UserId = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;
            var result = _lossMonitorService.InsertSMA(criteria);

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.LossMonitor, Policy = Constant.Policy.Host)]
        public JsonResult Update(UpdateSMAMaxLossCriteria criteria)
        {
            criteria.UserId = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;
            var result = _lossMonitorService.UpdateMaxLoss(criteria);

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.LossMonitor, Policy = Constant.Policy.Host)]
        public JsonResult Remove(DeleteSMACriteria criteria)
        {
            criteria.UserId = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;
            var result = _lossMonitorService.DeleteSMA(criteria);

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.LossMonitor, Policy = Constant.Policy.Host)]
        public JsonResult UpdateLetsTalkNotification(UpdateLetsTalkNotificationCriteria criteria)
        {
            criteria.UserId = CurrentSession.UserId;
            criteria.IP = CurrentContext.IP;
            var result = _lossMonitorService.UpdateLetsTalkNotification(criteria);

            return Json(result, JsonRequestBehavior.DenyGet);
        }
    }
}