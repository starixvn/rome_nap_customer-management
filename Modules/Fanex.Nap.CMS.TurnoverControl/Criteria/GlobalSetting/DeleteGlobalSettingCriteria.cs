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
    using Fanex.Nap.CMS.Infrastructure.Helpers;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Delete Global Setting List Criteria.
    /// </summary>
    [Serializable]
    public class DeleteGlobalSettingCriteria : CriteriaBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IdNameModel RoleInfo { get; set; }

        public IdNameModel Currency { get; set; }

        public decimal MaxTurnover { get; set; }

        public int UserId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

        public string IP
        {
            get
            {
                return IPHelper.ClientIP();
            }
        }

        public override string GetSettingKey()
        {
            return "TurnoverControl_GlobalSetting_Delete";
        }

        public override bool IsValid()
        {
            return true;
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
