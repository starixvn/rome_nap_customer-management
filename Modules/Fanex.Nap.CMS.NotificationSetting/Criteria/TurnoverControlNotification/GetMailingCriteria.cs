// -----------------------------------------------------------------------
// <copyright file="GetMailingCriteria.cs">
// Copyright (c). All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Nap.CMS.NotificationSetting.Criteria.TurnoverControlNotification
{
    using Fanex.Nap.CMS.Infrastructure.Configurations;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetMailingCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "FuncID")]
        public int FunctionId
        {
            get
            {
                return AppSettings.SMATurnoverMailFunctionId;
            }
        }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Acc_MailingListSel";
        }
    }
}