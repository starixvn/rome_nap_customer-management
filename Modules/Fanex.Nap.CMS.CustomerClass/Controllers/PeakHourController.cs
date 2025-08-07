namespace Fanex.Nap.CMS.CustomerClass.Controllers
{
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using Fanex.Nap.CMS.CustomerClass.Requests.PeakHour;
    using Fanex.Nap.CMS.CustomerClass.Services;
    using Fanex.Nap.CMS.CustomerClass.ViewModels;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.NAP.Client.Helpers.Business;
    using Fanex.NAP.Client.Attributes;
    using System.Linq;
    using System.Web.Mvc;


    public class PeakHourController : SportbookBaseController
    {
        private readonly IFilterService filterService;
        private readonly ISpreadService spreadService;
        private readonly IPeakHourService peakHourService;
        private const byte SpreadType = 2;

        public PeakHourController() : this(new FilterService(), new SpreadService(SpreadType), new PeakHourService())
        {
        }

        internal PeakHourController(IFilterService filterService, ISpreadService spreadService, IPeakHourService peakHourService)
        {
            this.filterService = filterService;
            this.spreadService = spreadService;
            this.peakHourService = peakHourService;
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.PeakHourManagement, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var havePermissionPortTypes = GetHavePermissionSportTypes(filterService.GetSports(), Constant.Policy.Read);
            if (!havePermissionPortTypes.Any())
            {
                return View("~/Views/Error/AccessDenied.cshtml");
            }

            var viewModel = new PeakHourViewModel(
                   HasRight(CMSFunctionKeys.PeakHourManagement, Constant.Policy.Host),
                   filterService.GetCustomerClasses(),
                   filterService.GetAllSites(),
                   filterService.GetAllCurrencies(), 
                   havePermissionPortTypes,
                   filterService.GetLeagueGroups(havePermissionPortTypes.Select(sp => sp.Id)));

            return View("~/Areas/CustomerClass/Views/PeakHour/Index.cshtml", viewModel);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.PeakHourManagement, Policy = Constant.Policy.Read)]
        public ActionResult Search(SearchSpreadRuleRequest request)
        {
            var havePermissionPortTypes = GetHavePermissionSportTypes(filterService.GetSports(), Constant.Policy.Read).Where(sp => request.SportTypes.Split(',').Contains(sp.Id.ToString()) || request.SportTypes == "0");
            if (!havePermissionPortTypes.Any())
            {
                return Forbidden(behavior: JsonRequestBehavior.AllowGet);
            }
            var leagueGroups = filterService.GetLeagueGroups(havePermissionPortTypes.Select(sp => sp.Id));

            return JsonData(peakHourService.GetSpreadRules(request, havePermissionPortTypes, leagueGroups), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.PeakHourManagement, Policy = Constant.Policy.Host)]
        public ActionResult Add(AddEditSpreadRuleRequest request)
        {
            if (!HavePermissionOnSportType(request.SportType, Constant.Policy.Read))
            {
                return Forbidden();
            }

            return JsonData(spreadService.AddRule(request, CurrentSession.UserId, CurrentContext.IP));
        }


        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.PeakHourManagement, Policy = Constant.Policy.Host)]
        public ActionResult Update(AddEditSpreadRuleRequest request)
        {

            if (!HavePermissionOnSportType(request.SportType, Constant.Policy.Read))
            {
                return Forbidden();
            }

            return JsonData(spreadService.UpdateRule(request, CurrentSession.UserId, CurrentContext.IP), JsonRequestBehavior.DenyGet);
        }


        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.PeakHourManagement, Policy = Constant.Policy.Host)]
        public ActionResult Delete(int policyID, short sportType)
        {
            if (!HavePermissionOnSportType(sportType, Constant.Policy.Read))
            {
                return Forbidden();
            }

            return JsonData(spreadService.DeleteRule(policyID, CurrentSession.UserId, CurrentContext.IP), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.PeakHourManagement, Policy = Constant.Policy.Host)]
        public ActionResult Enable(int policyID, short sportType, bool enable)
        {
            if (!HavePermissionOnSportType(sportType, Constant.Policy.Read))
            {
                return Forbidden();
            }

            return JsonData(spreadService.EnableRule(policyID, enable, CurrentSession.UserId, CurrentContext.IP), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.PeakHourManagement, Policy = Constant.Policy.Read)]
        public ActionResult GetPeakHour()
        {
            return JsonData(peakHourService.GetPeakHours(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.PeakHourManagement, Policy = Constant.Policy.Host)]
        public ActionResult AddPeakHour(AddEditPeakHourRequest request)
        {
            return JsonData(peakHourService.AddPeakHour(request, CurrentSession.UserId, CurrentContext.IP), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.PeakHourManagement, Policy = Constant.Policy.Host)]
        public ActionResult EditPeakHour(AddEditPeakHourRequest request)
        {
            return JsonData(peakHourService.UpdatePeakHour(request, CurrentSession.UserId, CurrentContext.IP), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.PeakHourManagement, Policy = Constant.Policy.Host)]
        public ActionResult DeletePeakHour(int rangeId)
        {
            return JsonData(peakHourService.DeletePeakHour(rangeId, CurrentSession.UserId, CurrentContext.IP), JsonRequestBehavior.DenyGet);
        }
    }
}
