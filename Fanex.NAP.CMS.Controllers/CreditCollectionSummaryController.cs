namespace Fanex.NAP.CMS.Controllers
{
    using System.Web.Mvc;
    using Exporter;
    using Models.CreditCollectionSummary;
    using Nap.CMS.Infrastructure;
    using Nap.CMS.Infrastructure.Web;
    using NAP.Core.Business;
    using NAP.Core.Web.Attribute;
    using Services.Implements;
    using Services.Interfaces;

    public class CreditCollectionSummaryController : BaseController
    {
        private readonly ICreditCollectionSummaryService _creditCollectionSummary;

        public CreditCollectionSummaryController() :
            this(null)
        {
        }

        public CreditCollectionSummaryController(ICreditCollectionSummaryService creditCollectionSummary)
        {
            _creditCollectionSummary = creditCollectionSummary ?? new CreditCollectionSummaryService();
            ViewBag.HasEditAccessRight = HasRight(CMSFunctionKeys.CreditCollection, Constant.Policy.Host);
        }

        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.CreditCollection, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.CreditCollection, Policy = Constant.Policy.Read)]
        public JsonResult Generate(CreditCollectionSummaryFilter filter)
        {
            var balanceUpdate = AppSession.BalanceUpDate;
            filter.MoveBetsDay = balanceUpdate.MoveBetsDay;
            filter.BalanceUpDay = balanceUpdate.BalanceUpDay;

            var result = _creditCollectionSummary.Generate(filter);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.CreditCollection, Policy = Constant.Policy.Host)]
        public JsonResult Save(SaveCreditStatusParam param)
        {
            param.UserId = CurrentSession.UserId;
            param.IP = CurrentContext.IP;

            var result = _creditCollectionSummary.Save(param);

            return Json(result);
        }

        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.CreditCollection, Policy = Constant.Policy.Read)]
        public void Export(CreditCollectionSummaryFilter filter)
        {
            var balanceUpdate = AppSession.BalanceUpDate;
            filter.MoveBetsDay = balanceUpdate.MoveBetsDay;
            filter.BalanceUpDay = balanceUpdate.BalanceUpDay;

            var result = _creditCollectionSummary.Generate(filter);

            var exporter = new CreditCollectionSummaryExporter(result, filter);
            exporter.ResponseStream();
        }
    }
}
