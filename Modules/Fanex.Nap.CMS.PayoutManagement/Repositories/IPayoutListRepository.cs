namespace Fanex.Nap.CMS.PayoutManagement.Repositories
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.PayoutManagement.Models;

    public interface IPayoutListRepository
    {
        void GetSitesLicenseesProducts(
            CriteriaBase criteria,
            out IList<GroupIdNameModel> sitesLicensees,
            out IList<ProductModel> products);

        IEnumerable<PayoutItem> GetPayoutList(CriteriaBase criteria);

        PayoutDetail GetPayoutDetail(CriteriaBase criteria, bool viewSecurityCode);

        Result UpdateTicketStatus(CommandBaseCriteria criteria);
    }
}
