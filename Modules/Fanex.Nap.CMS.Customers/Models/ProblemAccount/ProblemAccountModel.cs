namespace Fanex.Nap.CMS.Customers.Models.ProblemAccount
{
    using System.Collections.Generic;

    public class ProblemAccountModel
    {
        public ProblemAccountModel(
            int totalItems,
            IEnumerable<ProblemAccountItem> problemAccounts)
        {
            TotalItems = totalItems;
            ProblemAccounts = problemAccounts;
        }

        public int TotalItems { get; }

        public IEnumerable<ProblemAccountItem> ProblemAccounts { get; }
    }
}
