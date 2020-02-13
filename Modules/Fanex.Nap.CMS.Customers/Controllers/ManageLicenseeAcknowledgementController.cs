namespace Fanex.Nap.CMS.Customers.Controllers
{
    using System.Web.Mvc;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Infrastructure;
    using Infrastructure.Controllers;
    using Services;

    public class ManageLicenseeAcknowledgementController : BaseController
    {
        private readonly IProblemAccountLicenseeSiteService service;

        public ManageLicenseeAcknowledgementController()
            : this(null)
        {
        }

        public ManageLicenseeAcknowledgementController(IProblemAccountLicenseeSiteService service)
        {
            this.service = service ?? new ProblemAccountLicenseeSiteService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.ProblemAccountManageLicenseeAcknowledgement, Policy = Constant.Policy.Read)]
        public ActionResult Index()
            => View(
                "~/Areas/Customers/Views/ManageLicenseeAcknowledgement/Index.cshtml",
                service.GetLicenseeAcknowledgements());

        [NapAuth(FunctionKey = CMSFunctionKeys.ProblemAccountManageLicenseeAcknowledgement, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult Update(int licenseeId, bool isAcknowledgement, bool? confirmed = null)
            => licenseeId <= 0
            ? BadRequest()
            : JsonData(
                service.UpdateLicenseeAcknowledgement(licenseeId, isAcknowledgement, confirmed ?? false, CurrentSession.UserId, CurrentContext.IP),
                JsonRequestBehavior.DenyGet);
    }
}
