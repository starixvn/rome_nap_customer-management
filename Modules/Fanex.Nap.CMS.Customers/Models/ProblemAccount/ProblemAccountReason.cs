namespace Fanex.Nap.CMS.Customers.Models.ProblemAccount
{
    using System;

    public class ProblemAccountReason
    {
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

        public int CustId { get; }

        public string Username { get; }

        public DateTime CreateDate { get; }

        public string Reason { get; }
    }
}
