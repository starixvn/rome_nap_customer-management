// --------------------------------------------------------------------------
// <copyright file="LossMonitorService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.23">Created</change>
// </history>

namespace Fanex.NAP.CMS.Services.Implements
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.NAP.CMS.Core.Resources;
    using Fanex.NAP.CMS.Criteria.LossMonitor;
    using Fanex.NAP.CMS.Models.LossMonitor;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.CMS.ViewModels.LossMonitor;

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
            IEnumerable<SMALossInfo> rawData = null;
            List<SMALossGroup> result = null;

            rawData = this.GetData<SMALossInfo>(criteria);

            if (rawData != null && rawData.Any())
            {
                IEnumerable<IGrouping<byte, SMALossInfo>> groups = rawData.GroupBy(obj => obj.LevelID);
                SMALossGroup lossGroup = null;
                SMALossInfo firstItem = null;
                SMALossInfo previousGroupFirstItem = null;

                result = new List<SMALossGroup>();

                foreach (IGrouping<byte, SMALossInfo> group in groups)
                {
                    firstItem = group.ElementAt(0);
                    lossGroup = new SMALossGroup(firstItem);

                    foreach (SMALossInfo item in group)
                    {
                        lossGroup.SMALossItems.Add(new SMALossItem(item));
                    }

                    if (lossGroup.SMALossItems.Count > 0)
                    {
                        if (previousGroupFirstItem != null)
                        {
                            lossGroup.LossLevelRange = string.Format("{0}% - {1}%", firstItem.Value, previousGroupFirstItem.Value - 1);
                        }
                        else
                        {
                            lossGroup.LossLevelRange = string.Format("{0}% & {1}", firstItem.Value, ViewLang.Above);
                        }

                        result.Add(lossGroup);
                    }

                    previousGroupFirstItem = firstItem;
                }
            }

            return result;
        }
    }
}