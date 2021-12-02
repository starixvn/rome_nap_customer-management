namespace Fanex.Nap.CMS.Customers.Models.ProblemAccount
{
    using System.Collections.Generic;

    public class CTSUpdateProblemAccount
    {
        public CTSUpdateProblemAccount(IEnumerable<CTSCustomerReason> custInfos, int userId)
        {
            CustInfos = custInfos;
            UserId = userId;
        }

        public IEnumerable<CTSCustomerReason> CustInfos { get; }

        public int UserId { get; }
    }
}
