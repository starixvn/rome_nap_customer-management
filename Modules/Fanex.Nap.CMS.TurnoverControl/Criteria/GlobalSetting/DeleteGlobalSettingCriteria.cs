// --------------------------------------------------------------------------
// <copyright file="DeleteGlobalSettingCriteria.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting
{
    using System;
    using System.Collections;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Delete Global Setting List Criteria.
    /// </summary>
    [Serializable]
    public class DeleteGlobalSettingCriteria : EditGlobalSettingCriteria
    {
        public override string GetSettingKey()
        {
            return "TurnoverControl_GlobalSetting_Delete";
        }

        /// <summary>
        /// Gets the SP parameters.
        /// </summary>
        /// <returns>Store Procedure Parameters.</returns>
        public override Hashtable GetSpParams()
        {
            Hashtable storeProcedureParams = new Hashtable();
            storeProcedureParams.Add("custid", Id);
            storeProcedureParams.Add("ip", IP);
            storeProcedureParams.Add("userid", UserId);
            storeProcedureParams.Add("errcode", 0);
            storeProcedureParams.Add("errmsg", string.Empty);
            return storeProcedureParams;
        }
    }
}
