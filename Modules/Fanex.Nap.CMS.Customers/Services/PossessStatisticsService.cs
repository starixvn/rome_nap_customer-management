namespace Fanex.Nap.CMS.Customers.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.UI.WebControls.Expressions;
    using Fanex.Nap.CMS.Customers.Criteria.PossessStatistics;
    using Fanex.Nap.CMS.Customers.Params.PossessStatistics;
    using Fanex.Nap.CMS.Customers.Resources;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Models.PossessStatistics;

    public interface PossessStatisticsService
    {
        Tuple<int, IEnumerable<PossessStatisticsModel>> GetPossessStatistics(GetPossessStatisticsParam param);

        Result UpdateDangerLevel(UpdateDangerLevelParam param);

        Result UpdateMonitorStatus(UpdateMonitorStatusParam param);
    }

    public class PossessStatisticsServiceImpl : ServiceBase, PossessStatisticsService
    {
        public Tuple<int, IEnumerable<PossessStatisticsModel>> GetPossessStatistics(GetPossessStatisticsParam param)
        {
            var criteria = new GetPossessStatisticsCriteria(
                param.SBFrom,
                param.SBTo,
                param.NSFrom,
                param.NSTo,
                param.Username,
                param.AgentStatus,
                param.DangerLevel,
                param.MonitorPT,
                param.SortBy,
                param.FilterPossess,
                param.FilterDanger,
                param.PageIndex,
                param.PageSize);
            var possessStatistics = DynamicRepository.Fetch<PossessStatisticsModel>(criteria);

            return Tuple.Create(criteria.TotalItems, possessStatistics);
        }

        public Result UpdateDangerLevel(UpdateDangerLevelParam param)
        {
            var failedUsernames = new List<string>();

            Parallel.ForEach(param.SelectedCustomers, customer =>
            {
                var criteria = new UpdateDangerLevelCritera
                {
                    CustId = customer.Key,
                    UpdateDownline = param.UpdateDownline,
                    DangerType = param.DangerType,
                    DangerLevel = param.DangerLevel,
                    UserId = param.UserId,
                    IP = param.IP
                };

                var result = DoAction(criteria);

                if (!result.IsSuccess)
                {
                    failedUsernames.Add(customer.Value);
                }
            });

            if (failedUsernames.Any())
            {
                return new Result(1, string.Format(PossessStatisticsResource.UpdateErrorMessage, string.Join(", ", failedUsernames)));
            }

            return new Result { Message = PossessStatisticsResource.UpdateSuccessfull };
        }

        public Result UpdateMonitorStatus(UpdateMonitorStatusParam param)
        {
            var criteria = new UpdateMonitorStatusCriteria
            {
                CustIdList = param.CustomerList,
                IsMonitorPT = param.IsMonitored,
                UserId = param.UserId,
                IP = param.IP
            };

            var result = DoAction(criteria);

            if (result.IsSuccess)
            {
                result.Message = PossessStatisticsResource.UpdateSuccessfull;
            }

            return result;
        }
    }
}