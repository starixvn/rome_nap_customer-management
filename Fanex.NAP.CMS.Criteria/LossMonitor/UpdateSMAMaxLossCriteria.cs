// --------------------------------------------------------------------------
// <copyright file="UpdateSMAMaxLossCriteria.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Aaron.Cao" date="2016.06.07">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.LossMonitor
{
    using Fanex.Data.Repository;

    public class UpdateSMAMaxLossCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int MaxLoss { get; set; }

        public string IP { get; set; }

        public int Creator { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_LimitLoss_SMAMaxLossUpd";
        }

        public override bool IsValid()
        {
            return MaxLoss > 0;
        }
    }
}
