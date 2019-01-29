namespace Fanex.NAP.CMS.Criteria.CreditCollectionSummary
{
    using Data.Repository;

    public class SaveCreditStatusCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int Status { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CreditCollStatusUpd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
