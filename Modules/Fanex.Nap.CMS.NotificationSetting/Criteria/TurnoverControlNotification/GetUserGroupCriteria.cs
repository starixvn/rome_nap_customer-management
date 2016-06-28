// -----------------------------------------------------------------------
// <copyright file="GetUserGroupCriteria.cs">
// Copyright (c). All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Nap.CMS.NotificationSetting.Criteria.TurnoverControlNotification
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetUserGroupCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "NAP_User_GetById";
        }
    }
}