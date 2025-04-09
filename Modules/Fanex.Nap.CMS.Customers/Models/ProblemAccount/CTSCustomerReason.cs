namespace Fanex.Nap.CMS.Customers.Models.ProblemAccount
{
    public class CTSCustomerReason
    {
        public CTSCustomerReason()
        {
        }

        public CTSCustomerReason(int custId, int? fromReasonID, int toReasonID)
        {
            CustID = custId;
            FromReasonID = fromReasonID;
            ToReasonID = toReasonID;
        }
        public int CustID { get; set; }

        public int? FromReasonID { get; set; }

        public int ToReasonID { get; set; }
    }
}
