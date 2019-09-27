namespace Fanex.Nap.CMS.Customers.Controllers
{
    using System.Web.Mvc;
    using Constants;
    using Fanex.Nap.CMS.Customers.Params.PossessStatistics;
    using Infrastructure.Controllers;
    using NAP.Core.Business;
    using NAP.Core.Web.Attribute;
    using Services;
    using ViewModels.PossessStatistics;

    public class PossessStatisticsController : BaseController
    {
        private readonly PossessStatisticsService service;

        public PossessStatisticsController()
            : this(null)
        {
        }

        public PossessStatisticsController(PossessStatisticsService service)
        {
            this.service = service ?? new PossessStatisticsServiceImpl();
        }

        [NapAuth(FunctionKey = CustomerFunctionKeys.PossessStatisticsCN88, Policy = Constant.Policy.Read)]
        public ActionResult Index()
            => View(
                "~/Areas/Customers/Views/PossessStatistics/Index.cshtml",
                new IndexViewModel(HasRight(CustomerFunctionKeys.PossessStatisticsCN88, Constant.Policy.Host)));

        [NapAuth(FunctionKey = CustomerFunctionKeys.PossessStatisticsCN88, Policy = Constant.Policy.Read)]
        public ActionResult GetPossessStatistics(FilterViewModel vm)
        {
            var data = service.GetPossessStatistics(
                new GetPossessStatisticsParam(
                    vm.TotalSBPercentFrom,
                    vm.TotalSBPercentTo,
                    vm.TotalNSPercentFrom,
                    vm.TotalNSPercentTo,
                    vm.Username,
                    vm.AgentStatus,
                    vm.DangerLevelValue,
                    vm.Monitored,
                    vm.SortBy,
                    vm.PTConditionLogic,
                    vm.DangerLevelOperator,
                    vm.PageIndex,
                    vm.PageSize));

            return JsonData(new { TotalItems = data.Item1, PossessStatistics = data.Item2 });
        }

        [HttpPost]
        [NapAuth(FunctionKey = CustomerFunctionKeys.PossessStatisticsCN88, Policy = Constant.Policy.Host)]
        public ActionResult UpdateDangerLevel(UpdateDangerLevelViewModel vm)
            => JsonData(
                service.UpdateDangerLevel(
                    new UpdateDangerLevelParam(
                        vm.SelectedCustomers,
                        vm.ApplyForAllMembers,
                        vm.Category,
                        vm.DangerLevel,
                        CurrentSession.UserId,
                        CurrentContext.IP)),
                JsonRequestBehavior.DenyGet);

        [HttpPost]
        [NapAuth(FunctionKey = CustomerFunctionKeys.PossessStatisticsCN88, Policy = Constant.Policy.Host)]
        public ActionResult UpdateMonitorStatus(UpdateMonitorStatusViewModel vm)
            => JsonData(
                service.UpdateMonitorStatus(
                    new UpdateMonitorStatusParam(
                        vm.SelectedCustIds,
                        vm.MonitorStatus,
                        CurrentSession.UserId,
                        CurrentContext.IP)),
                JsonRequestBehavior.DenyGet);

        [NapAuth(FunctionKey = CustomerFunctionKeys.PossessStatisticsCN88, Policy = Constant.Policy.Read)]
        public void ExportExcel(FilterViewModel vm)
        {
            var data = service.GetPossessStatistics(
                 new GetPossessStatisticsParam(
                    vm.TotalSBPercentFrom,
                    vm.TotalSBPercentTo,
                    vm.TotalNSPercentFrom,
                    vm.TotalNSPercentTo,
                    vm.Username,
                    vm.AgentStatus,
                    vm.DangerLevelValue,
                    vm.Monitored,
                    vm.SortBy,
                    vm.PTConditionLogic,
                    vm.DangerLevelOperator,
                    1,
                    int.MaxValue));

            var exporter = new PossessStatisticsExporter(data);
            exporter.ResponseStream();
        }
    }
}