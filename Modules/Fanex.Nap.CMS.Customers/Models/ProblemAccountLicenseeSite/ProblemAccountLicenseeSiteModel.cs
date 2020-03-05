namespace Fanex.Nap.CMS.Customers.Models.ProblemAccountLicenseeSite
{
    using System.Collections.Generic;

    public class ProblemAccountLicenseeSiteModel
    {
        public ProblemAccountLicenseeSiteModel(
           int totalItems,
           IEnumerable<ProblemAccountSiteGroup> problemAccounts)
        {
            TotalItems = totalItems;
            ProblemAccounts = problemAccounts;
        }

        public int TotalItems { get; }

        public IEnumerable<ProblemAccountSiteGroup> ProblemAccounts { get; }
    }
}
