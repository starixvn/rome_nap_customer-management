namespace Fanex.Nap.CMS.PayoutManagement.Services
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.PayoutManagement.Criteria;
    using Fanex.Nap.CMS.PayoutManagement.Models;

    public interface IPayoutListService
    {
        void GetSitesLicenseesProducts(out IList<GroupIdNameModel> sitesLicensees, out IList<ProductModel> products);

        string GetGroupSitenamePermissionKey(string sitename);

        IEnumerable<PayoutItem> GetPayoutList(GetPayoutListCriteria criteria);

        PayoutDetail GetPayoutDetail(CriteriaBase criteria, bool viewSecurityCode);

        Result UpdateTicketStatus(CommandBaseCriteria criteria);
    }
}
