// --------------------------------------------------------------------------
// <copyright file="LossMonitorService.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.23">Created</change>
//   <change who="Aaron.Cao" date="2016.6.7">Added Insert, Update, Delete</change>
// </history>

namespace Fanex.NAP.CMS.Services.Implements
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.NAP.CMS.Criteria.LossMonitor;
    using Fanex.NAP.CMS.Models.LossMonitor;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.CMS.ViewModels.LossMonitor;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// Loss monitor service.
    /// </summary>
    public class LossMonitorService : CMSBaseService, ILossMonitorService
    {
        /// <summary>
        /// Gets the list of SMA who reached the max loss limit.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of SMALossGroup objects.</returns>
        public List<SMALossGroup> GetSMALoss(GetSMALossMonitorListCriteria criteria)
        {
            IEnumerable<SMALossInfo> rawData = this.Repository.Fetch<SMALossInfo>(criteria);
            List<SMALossLevelItem> levels = this.GetSMALossLevels();
            List<SMALossGroup> result = null;

            if (rawData != null && rawData.Any() && levels != null && levels.Count > 0)
            {
                IEnumerable<IGrouping<byte, SMALossInfo>> groups = rawData.GroupBy(obj => obj.LevelID);
                SMALossGroup lossGroup = null;
                SMALossInfo firstItem = null;
                SMALossLevelItem level = null;

                result = new List<SMALossGroup>();

                foreach (IGrouping<byte, SMALossInfo> group in groups)
                {
                    firstItem = group.ElementAt(0);
                    level = levels.FirstOrDefault(obj => obj.LevelID == firstItem.LevelID);
                    lossGroup = new SMALossGroup(firstItem, level.Value);

                    foreach (SMALossInfo item in group)
                    {
                        lossGroup.SMALossItems.Add(new SMALossItem(item));
                    }

                    if (lossGroup.SMALossItems.Count > 0)
                    {
                        result.Add(lossGroup);
                    }
                }

                result = result.OrderByDescending(obj => obj.LossLevel).ToList();
            }

            return result;
        }

        /// <summary>
        /// Gets the SMA loss levels.
        /// </summary>
        /// <returns>List of SMALossLevelItem objects.</returns>
        private List<SMALossLevelItem> GetSMALossLevels()
        {
            IEnumerable<SMALossLevel> rawData = this.Repository.Fetch<SMALossLevel>(new GetSMALossLevelsCriteria());
            List<SMALossLevelItem> result = null;

            if (rawData.Any())
            {
                SMALossLevel higherLevel = null;

                rawData = rawData.OrderByDescending(obj => obj.LevelID);
                result = new List<SMALossLevelItem>();

                foreach (SMALossLevel level in rawData)
                {
                    result.Add(new SMALossLevelItem(level, higherLevel));
                    higherLevel = level;
                }
            }

            return result;
        }

        public ICommandResult InsertSMA(InsertSMACriteria criteria)
        {
            return this.UpdateData(criteria);
        }

        public ICommandResult DeleteSMA(DeleteSMACriteria criteria)
        {
            return this.UpdateData(criteria);
        }

        public ICommandResult UpdateMaxLoss(UpdateSMAMaxLossCriteria criteria)
        {
            return this.UpdateData(criteria);
        }
    }
}