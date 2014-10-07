// --------------------------------------------------------------------------
// <copyright file="GlobalSettingListCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Global Setting List Criteria.
    /// </summary>
    [Serializable]
    public class GlobalSettingListCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "username")]
        public string SMAName { get; set; }

        [SpParam(Ignored = true)]
        public string SortField { get; set; }

        [SpParam(Ignored = true)]
        public string SortDirection { get; set; }

        public override string GetSettingKey()
        {
            return "TurnoverControl_GlobalSetting_List";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
