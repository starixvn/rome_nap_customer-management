namespace Fanex.Nap.CMS.Customers.Models.ProblemAccount
{
    public class CTSCustomerReason
    {
        public CTSCustomerReason(int custId, int? fromReasonID, int toReasonID)
        {
            CustID = custId;
            FromReasonID = fromReasonID;
            ToReasonID = toReasonID;
        }

        public int CustID { get; }

        public int? FromReasonID { get; }

        public int ToReasonID { get; }
    }
}
