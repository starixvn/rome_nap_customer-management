namespace Fanex.Nap.CMS.Customers.Repositories.ProblemAccountLicenseeSite
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Models.ProblemAccount;
    using Models.ProblemAccountLicenseeSite;
    using ProblemAccount;

    public interface IProblemAccountLicenseeSiteRepository : IProblemAccountRepository
    {
        IEnumerable<LicenseeAcknowledgement> GetLicenseeAcknowledgements(ICriteria criteria);

        new Tuple<IEnumerable<ProblemAccountLicenseeSiteItem>, IEnumerable<ProblemAccountReason>, int> GetProblemAccounts(ICriteria criteria);
    }
}
