namespace Fanex.Nap.CMS.PayoutManagement.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.PayoutManagement.Criteria;
    using Fanex.Nap.CMS.PayoutManagement.Enums;
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

        public void GetSitesLicenseesProducts(out IList<GroupIdNameModel> sitesLicensees, out IList<IdNameModel> products)
        {
            sitesLicensees = SessionManager.Get<IList<GroupIdNameModel>>(SitesLicenseesSessionKey);
            products = SessionManager.Get<IList<IdNameModel>>(ProductsSessionKey);
            if (sitesLicensees == null || products == null)
            {
                ICriteria criteria = new GetSitesLicenseesProductsCriteria();
                _payoutRepository.GetSitesLicenseesProducts(criteria, out sitesLicensees, out products);

                SessionManager.Set<IList<GroupIdNameModel>>(SitesLicenseesSessionKey, sitesLicensees);
                SessionManager.Set<IList<IdNameModel>>(ProductsSessionKey, products);
            }
        }

        public string GetGroupSitenamePermissionKey(string sitename)
        {
            string groupSitenamePermissionKey = null;
            IList<GroupIdNameModel> sitesLicensees;
            IList<IdNameModel> products;
            GetSitesLicenseesProducts(out sitesLicensees, out products);
            if (sitesLicensees != null)
            {
                string groupSitename = null;
                foreach (GroupIdNameModel group in sitesLicensees)
                {
                    if (groupSitename != null)
                    {
                        break;
                    }

                    foreach (IdNameModel site in group.ChildItems)
                    {
                        if (site.Name.Equals(sitename, StringComparison.OrdinalIgnoreCase))
                        {
                            groupSitename = group.GroupName;
                            break;
                        }
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
        
        public IList<IdNameModel> GetStatusStateMachine(int status)
        {
            if (status == 0)
            {
                return new List<IdNameModel> 
                {
                    new IdNameModel { Id = 0, Name = PayoutManagementLabels.All },
                    new IdNameModel { Id = (int)PayoutStatus.Open, Name = PayoutManagementLabels.Open },
                    new IdNameModel { Id = (int)PayoutStatus.InfoVerified, Name = PayoutManagementLabels.InfoVerified },
                    new IdNameModel { Id = (int)PayoutStatus.Disbursed, Name = PayoutManagementLabels.Disbursed },
                    new IdNameModel { Id = (int)PayoutStatus.Completed, Name = PayoutManagementLabels.Completed },
                    new IdNameModel { Id = (int)PayoutStatus.Expired, Name = PayoutManagementLabels.Expired }
                };
            }

            PayoutStatus payoutStatus = (PayoutStatus)status;
            if (payoutStatus == PayoutStatus.Open)
            {
                return new List<IdNameModel> 
                {
                    new IdNameModel { Id = (int)PayoutStatus.Open, Name = PayoutManagementLabels.Open },
                    new IdNameModel { Id = (int)PayoutStatus.InfoVerified, Name = PayoutManagementLabels.InfoVerified },
                    new IdNameModel { Id = (int)PayoutStatus.Expired, Name = PayoutManagementLabels.Expired }
                };
            }

            if (payoutStatus == PayoutStatus.InfoVerified)
            {
                return new List<IdNameModel> 
                {
                    new IdNameModel { Id = (int)PayoutStatus.Open, Name = PayoutManagementLabels.Open },
                    new IdNameModel { Id = (int)PayoutStatus.InfoVerified, Name = PayoutManagementLabels.InfoVerified },
                    new IdNameModel { Id = (int)PayoutStatus.Disbursed, Name = PayoutManagementLabels.Disbursed },
                };
            }

            if (payoutStatus == PayoutStatus.Disbursed)
            {
                return new List<IdNameModel> 
                {
                    new IdNameModel { Id = (int)PayoutStatus.InfoVerified, Name = PayoutManagementLabels.InfoVerified },
                    new IdNameModel { Id = (int)PayoutStatus.Disbursed, Name = PayoutManagementLabels.Disbursed },
                    new IdNameModel { Id = (int)PayoutStatus.Completed, Name = PayoutManagementLabels.Completed },
                };    
            }

            if (payoutStatus == PayoutStatus.Completed)
            {
                return new List<IdNameModel> 
                {
                    new IdNameModel { Id = (int)PayoutStatus.Completed, Name = PayoutManagementLabels.Completed }
                };    
            }

            return new List<IdNameModel> 
            {
                new IdNameModel { Id = (int)PayoutStatus.Expired, Name = PayoutManagementLabels.Expired }
            };
        }

        public IList<PayoutItem> GetPayoutList(ICriteria criteria)
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
