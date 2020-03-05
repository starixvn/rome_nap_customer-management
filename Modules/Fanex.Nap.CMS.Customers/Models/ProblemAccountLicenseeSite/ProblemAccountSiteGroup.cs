namespace Fanex.Nap.CMS.Customers.Models.ProblemAccountLicenseeSite
{
    using System.Collections.Generic;

    public class ProblemAccountSiteGroup
    {
        public ProblemAccountSiteGroup(
            string site,
            string acknowledged,
            IEnumerable<ProblemAccountLicenseeSiteItem> problemAccounts)
        {
            Site = site;
            Acknowledged = acknowledged;
            ProblemAccounts = problemAccounts;
        }

        public string Site { get; }

        public string Acknowledged { get; }

        public IEnumerable<ProblemAccountLicenseeSiteItem> ProblemAccounts { get; }
    }
}
