// --------------------------------------------------------------------------
// <copyright file="DeleteSMACriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Aaron.Cao" date="2016.06.07">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.LossMonitor
{
    using Fanex.Data.Repository;

    public class DeleteSMACriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int Creator { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_LimitLoss_SMADel";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}
