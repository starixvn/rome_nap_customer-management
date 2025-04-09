namespace Fanex.Nap.CMS.PayoutManagement.Criteria
{
    using System;
    using System.Collections;
    using System.Globalization;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetPayoutListCriteria : CriteriaBase
    {
        public string Username { get; set; }

        [SpParam(SqlParamName = "SiteIds")]
        public string Site { get; set; }

        [SpParam(SqlParamName = "ProductIds")]
        public string Product { get; set; }
        
        [SpParam(SqlParamName = "PayoutStatusIds")]
        public string Status { get; set; }

        public string From { get; set; }

        public DateTime FromDate { get; set; }

        public string To { get; set; }

        public DateTime ToDate { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_PayoutTrans_SelList";
        }

        public override bool IsValid()
            => true;
    }
}
