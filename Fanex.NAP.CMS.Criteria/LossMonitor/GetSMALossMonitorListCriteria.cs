// --------------------------------------------------------------------------
// <copyright file="GetSMALossMonitorListCriteria.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.7">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.LossMonitor
{
    using Fanex.Data.Repository;

    /// <summary>
    /// The criteria that are used to get SMA loss monitor list.
    /// </summary>
    public class GetSMALossMonitorListCriteria : CriteriaBase
    {
        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "NAP_LimitLoss_MonitorList";
        }

        /// <summary>
        /// Determines whether this criteria is valid.
        /// </summary>
        /// <returns>Whether this criteria is valid or not.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}