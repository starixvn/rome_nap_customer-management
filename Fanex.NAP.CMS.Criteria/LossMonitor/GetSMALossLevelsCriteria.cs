// --------------------------------------------------------------------------
// <copyright file="GetSMALossLevelsCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.27">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.LossMonitor
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// The criteria that are used to get list of loss levels.
    /// </summary>
    public class GetSMALossLevelsCriteria : CriteriaBase
    {
        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "NAP_LimitLoss_ActionConfigSel";
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