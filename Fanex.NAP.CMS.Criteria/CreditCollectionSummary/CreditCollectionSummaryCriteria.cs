namespace Fanex.NAP.CMS.Criteria.CreditCollectionSummary
{
    using System;
    using Data.Repository;

    public class CreditCollectionSummaryCriteria : CriteriaBase
    {
        public string Code { get; set; }

        public string Status { get; set; }

        [SpParam(SqlParamName = "FromDate")]
        public DateTime LastBalanceDate { get; set; }

        [SpParam(SqlParamName = "ToDate")]
        public DateTime CurrentBalanceDate { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public DateTime MoveBetsDay { get; set; }

        [SpParam(SqlParamName = "Type")]
        public int AccountType { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalItems { get; set; }

        public override string GetSettingKey()
            => "Acc_Rpt_CreditCollection";

        public override bool IsValid()
            => true;
    }
}
