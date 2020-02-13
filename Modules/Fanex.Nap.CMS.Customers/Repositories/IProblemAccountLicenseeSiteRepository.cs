namespace Fanex.Nap.CMS.Customers.Repositories
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Models.ProblemAccount;
    using Models.ProblemAccountLicenseeSite;

    public interface IProblemAccountLicenseeSiteRepository : IProblemAccountRepository
    {
        IEnumerable<LicenseeAcknowledgement> GetLicenseeAcknowledgements(ICriteria criteria);

        new Tuple<IEnumerable<ProblemAccountLicenseeSiteItem>, IEnumerable<ProblemAccountReason>, int> GetProblemAccounts(ICriteria criteria);
    }
}
