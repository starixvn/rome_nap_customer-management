namespace Fanex.Nap.CMS.Customers.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.Nap.CMS.Customers.Criteria.PossessStatistics;
    using Fanex.Nap.CMS.Customers.Params.PossessStatistics;
    using Fanex.Nap.CMS.Customers.Resources;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Models.PossessStatistics;
    using Repositories;

    public interface PossessStatisticsService
    {
        Tuple<int, IEnumerable<PossessStatisticsModel>> GetPossessStatistics(GetPossessStatisticsParam param);

        Result UpdateDangerLevel(UpdateDangerLevelParam param);

        Result UpdateMonitorStatus(UpdateMonitorStatusParam param);
    }

    public class PossessStatisticsServiceImpl : PossessStatisticsService
    {
        private readonly PossessStatisticsRepository repository;

        public PossessStatisticsServiceImpl(PossessStatisticsRepository repository = null)
        {
            this.repository = repository ?? new PossessStatisticsRepositoryImpl();
        }

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
            var possessStatistics = repository.PossessStatistics(criteria);

            return Tuple.Create(criteria.TotalItems, possessStatistics);
        }

        public Result UpdateDangerLevel(UpdateDangerLevelParam param)
        {
            var failedUsernames = new List<string>();

            Parallel.ForEach(param.SelectedCustomers, customer =>
            {
                var criteria = new UpdateDangerLevelCritera(
                    customer.Key,
                    param.UpdateDownline,
                    param.DangerType,
                    param.DangerLevel,
                    param.UserId,
                    param.IP);

                var result = repository.UpdateDangerLevel(criteria);

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
            var result = repository.UpdateMonitorStatus(new UpdateMonitorStatusCriteria(param.CustomerList, param.IsMonitored, param.UserId, param.IP));

            if (result.IsSuccess)
            {
                result.Message = PossessStatisticsResource.UpdateSuccessfull;
            }

            return result;
        }
    }
}