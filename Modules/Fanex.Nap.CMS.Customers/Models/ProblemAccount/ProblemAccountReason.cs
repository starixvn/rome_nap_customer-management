namespace Fanex.Nap.CMS.Customers.Models.ProblemAccount
{
    using System;

    public class ProblemAccountReason
    {
        public ProblemAccountReason()
        {
        }

        public ProblemAccountReason(
            int custId,
            string username,
            DateTime createDate,
            string reason)
        {
            CustId = custId;
            Username = username;
            CreateDate = createDate;
            Reason = reason;
        }
        public int CustId { get; set; }

        public string Username { get; set; }

        public DateTime CreateDate { get; set; }

        public string Reason { get; set; }
    }
}
