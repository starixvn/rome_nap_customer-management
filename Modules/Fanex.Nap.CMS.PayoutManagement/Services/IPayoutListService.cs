namespace Fanex.Nap.CMS.PayoutManagement.Services
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.PayoutManagement.Models;

    public interface IPayoutListService
    {
        void GetSitesLicenseesProducts(out IList<GroupIdNameModel> sitesLicensees, out IList<IdNameModel> products);

        IList<IdNameModel> GetStatusStateMachine(int status);

        IList<PayoutItem> GetPayoutList(ICriteria criteria);

        PayoutDetail GetPayoutDetail(ICriteria criteria, bool viewSecurityCode);

        Result UpdateTicketStatus(ICriteria criteria);
    }
}
