// --------------------------------------------------------------------------
// <copyright file="InsertSMACriteria.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Aaron.Cao" date="2016.06.07">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.LossMonitor
{
    using Fanex.Data.Repository;

    public class InsertSMACriteria : CriteriaBase
    {
        public string Username { get; set; }

        public int MaxLoss { get; set; }

        [SpParam(Ignored = true)]
        public bool IsHeadline { get; set; }

        public int Creator { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_LimitLoss_SMAIns";
        }

        public override bool IsValid()
        {
            return MaxLoss > 0;
        }
    }
}
