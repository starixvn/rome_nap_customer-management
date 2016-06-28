// --------------------------------------------------------------------------
// <copyright file="GetSportsCriteria.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Sports Criteria.
    /// </summary>
    public class GetSportsCriteria : CriteriaBase
    {
        public string Mode { get; set; }

        public override string GetSettingKey()
        {
            return "TurnoverControl_TurnoverMonitor_SportList";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
