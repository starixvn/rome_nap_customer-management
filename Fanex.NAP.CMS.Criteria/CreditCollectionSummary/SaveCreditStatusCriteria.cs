namespace Fanex.NAP.CMS.Criteria.CreditCollectionSummary
{
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class SaveCreditStatusCriteria : CommandBaseCriteria
    {
        public int CustId { get; set; }

        public int Status { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        public string Language { get; set; }

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
