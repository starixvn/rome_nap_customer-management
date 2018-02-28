namespace Fanex.NAP.CMS.Services.Implements
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Resources;
    using Criteria.LossMonitor;
    using Data.Repository;
    using Framework.Command;
    using Interfaces;
    using Models.LossMonitor;
    using ViewModels.LossMonitor;

    public class LossMonitorService : CMSBaseService, ILossMonitorService
    {
        public IEnumerable<SMALossGroup> GetCustomerLoss()
        {
            var rawdata = Repository.Fetch<SMALossInfo>(new GetSMALossMonitorListCriteria());
            var lossLevels = GetCustomerLossLevels();

            if (!rawdata.Any() || lossLevels.Count == 0)
            {
                yield break;
            }

            string levelName;
            var levelLookup = rawdata.ToLookup(kvp => kvp.LevelId, kvp => kvp);

            foreach (var level in levelLookup)
            {
                if (lossLevels.TryGetValue(level.Key, out levelName))
                {
                    yield return new SMALossGroup(level.Key, levelName, level);
                }
            }
        }

        public IEnumerable<SMALossInfo> GetGroupDownlines(ICriteria criteria)
        {
            return !criteria.IsValid() ?
                Enumerable.Empty<SMALossInfo>() :
                Repository.Fetch<SMALossInfo>(criteria);
        }

        public ICommandResult InsertSMA(ICriteria criteria)
        {
            return !criteria.IsValid() ?
                new CommandResult { Code = 400, Message = Messages.BadRequest } :
                UpdateData(criteria);
        }

        public ICommandResult UpdateMaxLoss(ICriteria criteria)
        {
            return !criteria.IsValid() ?
                new CommandResult { Code = 400, Message = Messages.BadRequest } :
                UpdateData(criteria);
        }

        public ICommandResult UpdateLetsTalkNotification(ICriteria criteria)
        {
            return !criteria.IsValid() ?
                new CommandResult { Code = 400, Message = Messages.BadRequest } :
                UpdateData(criteria);
        }

        public ICommandResult DeleteSMA(ICriteria criteria)
        {
            return !criteria.IsValid() ?
                new CommandResult { Code = 400, Message = Messages.BadRequest } :
                UpdateData(criteria);
        }

        private static string LossLevelValueToName(int value, int? higherValue)
        {
            return higherValue == null ?
                string.Format("{0}% & {1}", value, ViewLang.Above) :
                string.Format("{0}% - {1}%", value, higherValue.Value - 1);
        }

        private IDictionary<byte, string> GetCustomerLossLevels()
        {
            IDictionary<byte, string> lossLevels = new Dictionary<byte, string>();
            var rawdata = Repository.Fetch<SMALossLevel>(new GetSMALossLevelsCriteria());

            if (rawdata.Any())
            {
                int? higherValue = null;
                rawdata = rawdata.OrderByDescending(l => l.LevelId);

                foreach (var level in rawdata)
                {
                    lossLevels.Add(level.LevelId, LossLevelValueToName(level.Value, higherValue));
                    higherValue = level.Value;
                }
            }

            return lossLevels;
        }
    }
}