namespace Fanex.Nap.CMS.PayoutManagement.Repositories
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.PayoutManagement.Models;

    public interface IPayoutListRepository
    {
        void GetSitesLicenseesProducts(
            ICriteria criteria,
            out IList<GroupIdNameModel> sitesLicensees,
            out IList<IdNameModel> products);

        IList<PayoutItem> GetPayoutList(ICriteria criteria);

        PayoutDetail GetPayoutDetail(ICriteria criteria, bool viewSecurityCode);

        Result UpdateTicketStatus(ICriteria criteria);
    }
}
