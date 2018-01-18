namespace Fanex.Nap.CMS.PayoutManagement.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.PayoutManagement.Criteria;
    using Fanex.Nap.CMS.PayoutManagement.Models;
    using Fanex.Nap.CMS.PayoutManagement.Repositories;
    using Fanex.Nap.CMS.PayoutManagement.Resources;

    public class PayoutListService : IPayoutListService
    {
        private const string SitesLicenseesSessionKey = "PayoutService_SitesLicensees";
        private const string ProductsSessionKey = "PayoutService_Products";

        private readonly IPayoutListRepository _payoutRepository;

        public PayoutListService(IPayoutListRepository payoutRepository = null)
        {
            _payoutRepository = payoutRepository ?? new PayoutListRepository();
        }

        public void GetSitesLicenseesProducts(out IList<GroupIdNameModel> sitesLicensees, out IList<ProductModel> products)
        {
            sitesLicensees = SessionManager.Get<IList<GroupIdNameModel>>(SitesLicenseesSessionKey);
            products = SessionManager.Get<IList<ProductModel>>(ProductsSessionKey);

            if (sitesLicensees == null || products == null)
            {
                IList<ProductModel> productRawData;
                ICriteria criteria = new GetSitesLicenseesProductsCriteria();
                _payoutRepository.GetSitesLicenseesProducts(criteria, out sitesLicensees, out productRawData);

                if (sitesLicensees != null && sitesLicensees.Count > 0)
                {
                    SessionManager.Set(SitesLicenseesSessionKey, sitesLicensees);
                }

                if (productRawData != null && productRawData.Count > 0)
                {
                    products = new List<ProductModel>();
                    var productNameGroup = productRawData.ToLookup(kvp => kvp.Name, kvp => kvp);

                    foreach (var group in productNameGroup)
                    {
                        products.Add(new ProductModel
                        {
                            Id = string.Join(",", group.Select(p => p.Id).ToArray()),
                            Name = group.Key
                        });
                    }

                    SessionManager.Set(ProductsSessionKey, products);
                }
            }
        }

        public string GetGroupSitenamePermissionKey(string sitename)
        {
            string groupSitenamePermissionKey = null;
            IList<GroupIdNameModel> sitesLicensees;
            IList<ProductModel> products;
            GetSitesLicenseesProducts(out sitesLicensees, out products);

            if (sitesLicensees != null)
            {
                IdNameModel site;
                string groupSitename = null;

                foreach (GroupIdNameModel group in sitesLicensees)
                {
                    site = group.ChildItems.FirstOrDefault(s => s.Name.Equals(sitename, StringComparison.OrdinalIgnoreCase));

                    if (site != null)
                    {
                        groupSitename = group.GroupName;
                        break;
                    }
                }

                if (groupSitename != null)
                {
                    if (groupSitename.Equals("Boping Licensees", StringComparison.OrdinalIgnoreCase))
                    {
                        groupSitenamePermissionKey = CMSFunctionKeys.PayoutForBopingLicensees;
                    }
                    else if (groupSitename.Equals("ONEWorks Licensees", StringComparison.OrdinalIgnoreCase))
                    {
                        groupSitenamePermissionKey = CMSFunctionKeys.PayoutForONEworksLicensees;
                    }
                    else if (groupSitename.Equals("Credit Licensees", StringComparison.OrdinalIgnoreCase))
                    {
                        groupSitenamePermissionKey = CMSFunctionKeys.PayoutForCredit;
                    }
                }
            }

            return groupSitenamePermissionKey;
        }

        public IEnumerable<PayoutItem> GetPayoutList(ICriteria criteria)
        {
            return _payoutRepository.GetPayoutList(criteria);
        }

        public PayoutDetail GetPayoutDetail(ICriteria criteria, bool viewSecurityCode)
        {
            var payoutDetail = _payoutRepository.GetPayoutDetail(criteria, viewSecurityCode);
            if (payoutDetail != null && !string.IsNullOrEmpty(payoutDetail.SecurityCode))
            {
                int index = payoutDetail.SecurityCode.IndexOf(',');
                if (index >= 0)
                {
                    payoutDetail.SecurityCode = payoutDetail.SecurityCode.Substring(++index);
                }
            }

            return payoutDetail;
        }

        public Result UpdateTicketStatus(ICriteria criteria)
        {
            var result = _payoutRepository.UpdateTicketStatus(criteria);
            if (result.Code == 0)
            {
                result.Message = PayoutManagementLabels.UpdateTicketStatusSuccess;
            }

            return result;
        }
    }
}
