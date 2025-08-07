namespace Fanex.Nap.CMS.DomainManagement.Controllers
{
    using System.Web.Mvc;
    using Fanex.Nap.CMS.DomainManagement.Requests;
    using Fanex.Nap.CMS.DomainManagement.Services;
    using Fanex.Nap.CMS.DomainManagement.ViewModels;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
    using Fanex.NAP.Client.Helpers.Business;
    using Fanex.NAP.Client.Attributes;

    public class DomainManagementController : BaseController
    {
        private readonly IDomainManagementService domainManagementService;
        private readonly IPermissionService permissionService;

        public DomainManagementController()
            : this(new DomainManagementService(), new PermissionService())
        {
        }

        protected DomainManagementController(
            IDomainManagementService domainManagementService,
            IPermissionService permissionService)
        {
            this.domainManagementService = domainManagementService;
            this.permissionService = permissionService;
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.DomainManagement, Policy = Constant.Policy.Read)]
        public ActionResult Index()
            => View(
                "~/Areas/DomainManagement/Views/Index.cshtml",
                new DomainManagementViewModel(
                    domainManagementService.GetSites(),
                    domainManagementService.GetCountries(),
                    HasRight(CMSFunctionKeys.DomainManagement, Constant.Policy.Host)));

        [NapAuth(FunctionKey = CMSFunctionKeys.DomainManagement, Policy = Constant.Policy.Read)]
        public ActionResult Search(SearchDomainQuery message)
            => JsonData(domainManagementService.GetDomains(message));

        [NapAuth(FunctionKey = CMSFunctionKeys.DomainManagement, Policy = Constant.Policy.Read)]
        public ActionResult Get(string domainId)
            => JsonData(domainManagementService.GetDomain(domainId));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.DomainManagement, Policy = Constant.Policy.Host)]
        public ActionResult AddOrEdit(AddOrEditDomainCommand message)
        {
            if (message.IsValid())
            {
                message.UserId = CurrentSession.UserId;
                message.IP = CurrentContext.IP;

                return JsonData(domainManagementService.AddOrEditDomain(message), JsonRequestBehavior.DenyGet);
            }

            return BadRequest();
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.DomainManagement, Policy = Constant.Policy.Host)]
        public ActionResult Remove(IdNameModel[] domains)
            => domains == null || domains.Length == 0
            ? BadRequest()
            : JsonData(
                domainManagementService.RemoveDomains(domains, CurrentSession.UserId, CurrentContext.IP),
                JsonRequestBehavior.DenyGet);

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!permissionService.HasSpecialSitePermission(CurrentSession.UserId))
            {
                filterContext.Result = Redirect("~/Error/AccessDenied");
            }
        }
    }
}
